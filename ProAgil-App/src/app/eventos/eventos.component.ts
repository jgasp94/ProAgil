import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventos: Evento[];
  imgLargura = 50;
  imgMargem = 2;
  mostrarImg = false;
  modalRef: BsModalRef;
  eventosFiltrado: Evento[];
  // tslint:disable-next-line: variable-name
  _friltroLista: string;
  registerForm: FormGroup;

  constructor(
    private eventoService: EventoService
    // tslint:disable-next-line: align
    , private modalService: BsModalService
    ) { }

  get friltroLista() {
    return this._friltroLista;
  }
  set friltroLista(value: string) {
    this._friltroLista = value;
    this.eventosFiltrado = this.friltroLista ? this.filtrarEventos(this.friltroLista) : this.eventos;
  }
openModal(template: TemplateRef<any>) {
  this.modalRef = this.modalService.show(template);
}
  ngOnInit() {
    this.getEventos();
  }
  alternarImg() {
    this.mostrarImg = !this.mostrarImg;
  }
  salvarAlteracao() {
    this.registerForm = new FormGroup({
      tema: new FormControl,
      local: new FormControl,
      dataEvento: new FormControl,
      qtdPessoas: new FormControl,
      urlImagem: new FormControl,
      telefone: new FormControl,
      email: new FormControl
    });
  }
  validation(){

  }
  getEventos() {

      this.eventoService.getAllEvento().subscribe(
      // tslint:disable-next-line: variable-name
      (_eventos: Evento[]) => {
        this.eventos = _eventos;
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
}
