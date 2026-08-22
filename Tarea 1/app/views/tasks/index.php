<?php
require APP_PATH . '/views/layout/header.php';

$statusLabels = [
    'pendiente' => 'Pendiente',
    'en_progreso' => 'En progreso',
    'completada' => 'Completada',
];
?>

<p><a href="<?= BASE_URL ?>/projects" class="back-link">&laquo; Volver a proyectos</a></p>

<div class="page-header">
    <h1>Tareas de "<?= htmlspecialchars($project['name']) ?>"</h1>
    <div class="table-actions">
        <a href="<?= BASE_URL ?>/reports/tasks/<?= $project['id'] ?>" class="btn btn-secondary">Descargar PDF</a>
        <a href="<?= BASE_URL ?>/tasks/create/<?= $project['id'] ?>" class="btn">+ Nueva tarea</a>
    </div>
</div>

<?php if (empty($tasks)): ?>
    <p class="muted">Este proyecto todavía no tiene tareas.</p>
<?php else: ?>
    <table class="table">
        <thead>
            <tr>
                <th>Título</th>
                <th>Descripción</th>
                <th>Vence</th>
                <th>Estado</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            <?php foreach ($tasks as $task): ?>
                <tr class="<?= $task['status'] === 'completada' ? 'row-done' : '' ?>">
                    <td><?= htmlspecialchars($task['title']) ?></td>
                    <td class="muted"><?= htmlspecialchars($task['description']) ?></td>
                    <td><?= $task['due_date'] ? htmlspecialchars($task['due_date']) : '—' ?></td>
                    <td><span class="badge badge-<?= htmlspecialchars($task['status']) ?>"><?= $statusLabels[$task['status']] ?? $task['status'] ?></span></td>
                    <td class="table-actions">
                        <form method="post" action="<?= BASE_URL ?>/tasks/toggle/<?= $task['id'] ?>">
                            <button type="submit" class="btn btn-sm btn-secondary">
                                <?= $task['status'] === 'completada' ? 'Reabrir' : 'Completar' ?>
                            </button>
                        </form>
                        <a href="<?= BASE_URL ?>/tasks/edit/<?= $task['id'] ?>" class="btn btn-sm btn-secondary">Editar</a>
                        <form method="post" action="<?= BASE_URL ?>/tasks/delete/<?= $task['id'] ?>"
                              onsubmit="return confirm('¿Eliminar esta tarea?');">
                            <button type="submit" class="btn btn-sm btn-danger">Eliminar</button>
                        </form>
                    </td>
                </tr>
            <?php endforeach; ?>
        </tbody>
    </table>
<?php endif; ?>

<?php require APP_PATH . '/views/layout/footer.php'; ?>
