import { BaseResponse } from './base.response';

export class PagedResponse<T> extends BaseResponse {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
    data: T[];

    constructor(currentPage: number, totalPages: number, pageSize: number, totalCount: number,
                hasPrevious: boolean, hasNext: boolean, data: T[], success: boolean, errorMessage: string){
        super(success, errorMessage);

        this.currentPage = currentPage;
        this.totalPages = totalPages,
        this.pageSize = pageSize;
        this.totalCount = totalCount;
        this.hasPrevious = hasPrevious;
        this.hasNext = hasNext;
        this.data = data;
    }
}
