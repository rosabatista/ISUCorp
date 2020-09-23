export class Pagination {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;

    constructor(currentPage: number, totalPages: number, pageSize: number,
                totalCount: number, hasPrevious: boolean, hasNext: boolean) {

        this.currentPage = currentPage;
        this.totalPages = totalPages,
        this.pageSize = pageSize;
        this.totalCount = totalCount;
        this.hasPrevious = hasPrevious;
        this.hasNext = hasNext;
    }
}
