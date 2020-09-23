import { Place } from './place';
import { Contact } from './contact';
import { BaseEntity } from './base.entity';

export class Reservation extends BaseEntity {
    date: Date;
    notes: string;
    rating?: number;
    favorite: boolean;
    contactId: number;
    contact: Contact;
    place: Place;
    placeId: number;
}
