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

export interface HttpResult<T> {
    statusCode: number;
    title: string | null;
    type: string | null;
    data: T | null;
    message: string | null;
    errors: ErrorDetail[] | null;
}

export interface ErrorDetail {
    name: string | null;
    message: string;
}