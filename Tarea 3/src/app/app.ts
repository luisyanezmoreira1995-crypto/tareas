import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TaskService } from './services/task.service';
import { ReportGeneratorService } from './reports/report-generator.service';
import { TaskReportDataProvider } from './reports/task-report-data-provider';

@Component({
  selector: 'app-root',
  imports: [FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = 'bloc_tareas';

  private readonly taskService = inject(TaskService);
  private readonly reportGenerator = inject(ReportGeneratorService);
  private readonly taskReportDataProvider = inject(TaskReportDataProvider);

  protected readonly lista_tareas = this.taskService.tasks$;
  protected nuevaTarea = '';

  newTask(): void {
    this.taskService.add(this.nuevaTarea);
    this.nuevaTarea = '';
  }

  eliminar(id: number): void {
    this.taskService.remove(id);
  }

  descargarPdf(): void {
    this.reportGenerator.generate(this.taskReportDataProvider, this.taskService.getAll(), 'reporte_tareas.pdf');
  }
}
