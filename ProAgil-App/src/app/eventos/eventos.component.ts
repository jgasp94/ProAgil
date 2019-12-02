import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { error } from 'util';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  tituloPagina = 'Eventos';
  dataEvento: string;
  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  typeRequester: string;
  titleModal : string;  
  imgLargura = 50;
  imgMargem = 2;
  mostrarImg = false;
  _filtroLista = '';
  registerForm: FormGroup;
  bodyDeletarEvento: string;
  arq : File;
  nomeArq : string;
  
  constructor(
    private eventoService: EventoService
    , private modalService: BsModalService
    , private fb: FormBuilder
    , private locale: BsLocaleService
    , private toastr : ToastrService 
    ) {
      this.locale.use('pt-br');
    }
    
    get filtroLista(): string {
      return this._filtroLista;
    }
    set filtroLista(value: string) {
      this._filtroLista = value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;;
    }
    openModal(template: any) {
      this.registerForm.reset();
      template.show();
    }
    ngOnInit() {
      this.validation();
      this.getEventos();
    }
    alternarImg() {
      this.mostrarImg = !this.mostrarImg;
    }
    enviarImagem(event: Event ){
      this.arq = event.srcElement.files;
      console.log(this.arq); 
    } 
    editarEvento(template: any, evento: Evento) {
      this.openModal(template);      
      this.typeRequester = 'PUT';
      this.evento = Object.assign({}, evento);
      this.nomeArq = evento.imagemUrl.toString();
      this.evento.imagemUrl = '';
      this.titleModal = 'Editar Evento';
      this.registerForm.patchValue(this.evento);
      this.eventoService.getEventoById(evento.id).subscribe(
        (eventoEditado: Evento) => {
          console.log(eventoEditado);
        },
        error => {
          this.toastr.error(`Ops! Ocorreu um erro: ${error}`);
        }
        );
      }
      novoEvento(template: any, evento: Evento) {
        this.openModal(template);
        this.evento = evento;
        this.typeRequester = 'POST';
        this.titleModal = 'Novo Evento';
      }
      validation() {
        this.registerForm = this.fb.group({
          tema: ['',
          [Validators.required, Validators.minLength(4), Validators.maxLength(50)]
        ],
        local: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
        dataEvento: ['',
        [Validators.required]],
        qtdPessoas: ['',
        [Validators.required, Validators.maxLength(1200)]],
        imagemUrl: ['', [Validators.required]],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]]
      });
    }
    getEventos() {
      this.eventoService.getAllEvento().subscribe(
        // tslint:disable-next-line: variable-name
        (_eventos: Evento[]) => {
          this.eventos = _eventos;
          this.eventosFiltrados = this.eventos;        
        }, error => {
          this.toastr.error(`Erro ao tentar Carregar eventos: ${error}`);
        }
        );
      }
      filtrarEventos(filtrarPor: string): Evento[] {
        filtrarPor = filtrarPor.toLocaleLowerCase();
        return this.eventos.filter(
          evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
          );
        }
        salvarAlteracao(template: any) {
          if (this.registerForm.valid) {
            if (this.typeRequester.toUpperCase() === 'POST') {
              this.evento = Object.assign({}, this.registerForm.value);          
              this.eventoService.postUpload(this.arq, this.arq[0].name).subscribe(
                ()=>{
                  this.getEventos();
                },
                error => {
                  this.toastr.error(`Ocorreu um erro ao enviar imagem: ${error}`);
                }
                );
                this.eventoService.postEvento(this.evento).subscribe(
                  (novoEvento: Evento) => {
                    console.log(novoEvento);
                    template.hide();
                    this.getEventos();
                    this.toastr.success('Evento inserido com sucesso!')
                  },
                  error => {
                    this.toastr.error(`Ocorreu o seguinte erro: ${error}`);
                  }
                  );            
                } else {
                  this.evento = Object.assign({ id: this.evento.id }, this.registerForm.value);                  
                  this.evento.imagemUrl = this.nomeArq;                 
                  const nomeArquivo = this.evento.imagemUrl.split('\\', 3);
                  this.evento.imagemUrl = nomeArquivo[2];
                  this.eventoService.postUpload(this.arq, nomeArquivo[2]).subscribe(
                    ()=>{    
                      this.getEventos();                  
                    }, 
                    error => {
                      this.toastr.error(`Erro ao atualizar imagem: ${error}`);
                    }  
                    );
                    this.eventoService.putEvento(this.evento).subscribe(
                      () => {
                        template.hide();
                        this.getEventos();
                        this.toastr.success('Evento Atualizado!')
                      },
                      error => {
                        this.toastr.error(`Erro ao atualizar Evento: ${error}`);
                      }
                      );
                    }
                  }
                }
                excluirEvento(evento: Evento, confirmTemplate: any) {
                  this.openModal(confirmTemplate);
                  this.evento = evento;
                  this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.id} - ${evento.tema}`;
                }
                confirmeDelete(comfirm: any) {
                  this.eventoService.deleteEvento(this.evento).subscribe(
                    () => {
                      comfirm.hide();        
                      this.getEventos();
                      this.toastr.success('Usuário apagado com sucesso');
                    },
                    error => {
                      this.toastr.error(`Ocorreu o seguinte erro: ${error}`);
                    }
                    )
                  }  
                }