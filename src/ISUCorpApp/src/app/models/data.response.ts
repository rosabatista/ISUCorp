import { BaseResponse } from './base.response';

export class DataResponse<T> extends BaseResponse {
    data: T;

    constructor(data: T, success: boolean, errorMessage: string){
        super(success, errorMessage);
        this.data = data;
    }
}
