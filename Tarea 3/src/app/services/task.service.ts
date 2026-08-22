import { Injectable, signal } from '@angular/core';
import { Task } from '../models/task.model';

/** SRP: única responsabilidad es guardar y mutar la lista de tareas. */
@Injectable({ providedIn: 'root' })
export class TaskService {
  private nextId = 3;

  private readonly tasks = signal<Task[]>([
    { id: 1, nombre: 'Levantarse 7 am' },
    { id: 2, nombre: 'Desayunar' },
  ]);

  readonly tasks$ = this.tasks.asReadonly();

  add(nombre: string): void {
    const trimmed = nombre.trim();

    if (!trimmed) {
      return;
    }

    this.tasks.update((list) => [...list, { id: this.nextId++, nombre: trimmed }]);
  }

  remove(id: number): void {
    this.tasks.update((list) => list.filter((task) => task.id !== id));
  }

  getAll(): Task[] {
    return this.tasks();
  }
}
