export interface ReportColumn {
  key: string;
  label: string;
}

export interface ReportData {
  title: string;
  columns: ReportColumn[];
  rows: Record<string, string>[];
}

/**
 * Contrato para transformar cualquier fuente de datos (TSource) en un
 * ReportData tabular. Cualquier implementación es sustituible por otra sin
 * romper al consumidor (LSP).
 */
export interface ReportDataProvider<TSource> {
  build(source: TSource): ReportData;
}
