import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import {defineLocale, BsLocaleService, ptBrLocale} from 'ngx-bootstrap';
import { error } from 'util';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventos: Evento[];
  evento: Evento;
  typeRequester: string;
  imgLargura = 50;
  imgMargem = 2;
  mostrarImg = false;
  eventosFiltrado: Evento[];
  // tslint:disable-next-line: variable-name
  _friltroLista: string;
  registerForm: FormGroup;
  bodyDeletarEvento: string;

  constructor(
    private eventoService: EventoService
    , private modalService: BsModalService
    , private fb: FormBuilder
    , private locale: BsLocaleService
    ) {
      this.locale.use('pt-br');
    }
    
    get friltroLista() {
      return this._friltroLista;
    }
    set friltroLista(value: string) {
      this._friltroLista = value;
      this.eventosFiltrado = this.friltroLista ? this.filtrarEventos(this.friltroLista) : this.eventos;
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
    editarEvento(template: any, evento: Evento) {
      this.openModal(template);
      this.evento = evento;
      this.typeRequester = 'PUT';
      this.registerForm.patchValue(this.evento);
      this.eventoService.getEventoById(evento.id).subscribe(
        (eventoEditado: Evento) => {
          console.log(eventoEditado);
        },
        error => {
          console.log(error);
        }
        );
      }
      novoEvento(template: any, evento: Evento){
      this.openModal(template);
      this.evento = evento;
      this.typeRequester = 'POST';
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
          this.eventosFiltrado = this.eventos;
          console.log(_eventos);
        }, error => {
          console.log(error);
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
            if(this.typeRequester.toUpperCase() === 'POST'){
              this.evento = Object.assign({}, this.registerForm.value);
              this.eventoService.postEvento(this.evento).subscribe(
                (novoEvento: Evento) => {
                  console.log(novoEvento);
                  template.hide();
                  this.getEventos();
                },
                error => {
                  console.log(error);
                }
                );
              } else{
                this.evento = Object.assign( {id : this.evento.id}, this.registerForm.value);
                this.eventoService.putEvento(this.evento).subscribe(
                  () => {
                    template.hide();
                    this.getEventos();
                  },
                  error => {
                    console.log(this.evento);
                    console.log(error);
                  }
                  );
              }
            }
          }
          excluirEvento(evento: Evento,  confirmTemplate: any){
            this.openModal(confirmTemplate);
            this.evento = evento;
            this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.id} - ${evento.tema}`;
          }
          confirmeDelete(comfirm: any){
            this.eventoService.deleteEvento(this.evento).subscribe(
              () => {
                comfirm.hide();
                this.getEventos();
              },
              error => {
                console.log(error);
              }
            )
          }
        }