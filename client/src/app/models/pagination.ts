export interface Pagination<T> {
  pageIndex: number
  pageSize: number
  totalItems: number
  totalPages: number
  data: T;
}
