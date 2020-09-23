import { Reservation } from './reservation';
import { BaseEntity } from './base.entity';

export class Place extends BaseEntity {
    name: string;
    description?: number;
    reservations: Reservation[];
}
