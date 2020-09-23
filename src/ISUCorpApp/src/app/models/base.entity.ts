export class BaseEntity {
    id?: number;
    addedAt: Date;
    modifiedAt: Date;

    constructor(addedAt: Date, modifiedAt: Date, id: number = null){
        this.id = id;
        this.addedAt = addedAt;
        this.modifiedAt = modifiedAt;
    }
}
