import {  Lote } from './Lote';
import { RedeSocial } from './RedeSocial';
import { Evento } from './Evento';

export interface Evento {
    id: number;
    local: string;
    dataEvento: Date;
    tema: string;
    qtdPessoas: number;
    imagemUrl: string;
    lotes: Lote[];
    redeSociais: RedeSocial[];
    palestranteEventos: Evento[];
}
