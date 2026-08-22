<?php

/**
 * Contrato para cualquier fuente de datos de un reporte.
 * Cualquier implementación debe poder sustituir a otra sin romper al
 * consumidor (Principio de Sustitución de Liskov).
 */
interface ReportDataProviderInterface
{
    public function getTitle(): string;

    /** @return array<string,string> Mapa clave => etiqueta de columna */
    public function getColumns(): array;

    /** @return array<int,array<string,mixed>> Filas indexadas por las claves de getColumns() */
    public function getRows(): array;
}
