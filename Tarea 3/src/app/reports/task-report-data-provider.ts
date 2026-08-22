import { Injectable } from '@angular/core';
import { Task } from '../models/task.model';
import { ReportData, ReportDataProvider } from './report-data-provider.model';

/**
 * SRP: su única responsabilidad es transformar tareas en la forma tabular
 * que espera un ReportRenderer. No conoce jsPDF ni el TaskService.
 */
@Injectable({ providedIn: 'root' })
export class TaskReportDataProvider implements ReportDataProvider<Task[]> {
  build(tasks: Task[]): ReportData {
    return {
      title: 'Reporte de tareas',
      columns: [{ key: 'nombre', label: 'Tarea' }],
      rows: tasks.map((task) => ({ nombre: task.nombre })),
    };
  }
}
