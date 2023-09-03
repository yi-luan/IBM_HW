import { MatPaginatorIntl } from '@angular/material/paginator';

export function CustomPaginator() {
  const customPaginatorIntl = new MatPaginatorIntl();
  customPaginatorIntl.getRangeLabel = (
    page: number,
    pageSize: number,
    length: number
  ): string => {
    if (length === 0 || pageSize === 0) {
      return `第 0 筆、共 ${length} 筆`;
    }

    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    const endIndex =
      startIndex < length
        ? Math.min(startIndex + pageSize, length)
        : startIndex + pageSize;

    return `第 ${startIndex + 1} - ${endIndex} 筆、共 ${length} 筆`;
  };
  customPaginatorIntl.itemsPerPageLabel = '每頁筆數：';

  return customPaginatorIntl;
}