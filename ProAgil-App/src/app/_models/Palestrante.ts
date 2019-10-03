import { RedeSocial } from './RedeSocial';
import { Evento } from './Evento';

export interface Palestrante {
     id: number ;
     nome: string;
     miniCurriculum: string;
     imagemUrl: string ;
     telefone: string ;
     email: string ;
     redeSociais: RedeSocial[] ;
     palestranteEventos: Evento[] ;
}
