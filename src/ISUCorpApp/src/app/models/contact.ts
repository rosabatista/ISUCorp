import { Reservation } from './reservation';
import { BaseEntity } from './base.entity';

export class Contact extends BaseEntity {
    name: string;
    type: string;
    phone: string;
    birthDate: Date;
    reservations: Reservation[];
}
