export interface PagingRequest {
    pageIndex: number;
    pageSize: number;
}
export interface PagingResponse<T> extends PagingRequest {
    data: T[];
    hasNext: boolean; 
}
export interface CountedPagingResponse<T> extends PagingResponse<T> {
    pageCount: number;
    totalCount: number;
}

export interface ErrorDetail {
    name: string | null;
    message: string;
}
export class AppResult<T> {
    isSuccess: boolean;
    data: T | null;
    message: string | null;
    errors: ErrorDetail[] | null;

    constructor(result: { status: number;  data: T | null; message: string | null; errors: ErrorDetail[] | null }) {
        this.isSuccess = result.status >= 200 && result.status < 300;
        this.data = result.data;
        this.message = result.message;
        this.errors = result.errors;
    }
}