<?php require APP_PATH . '/views/layout/header.php'; ?>

<p><a href="<?= BASE_URL ?>/tasks/index/<?= $project['id'] ?>" class="back-link">&laquo; Volver a tareas</a></p>

<div class="card form-card">
    <h1><?= $task ? 'Editar tarea' : 'Nueva tarea' ?></h1>
    <p class="muted">Proyecto: <?= htmlspecialchars($project['name']) ?></p>

    <?php if (!empty($errors)): ?>
        <div class="alert alert-error">
            <ul>
                <?php foreach ($errors as $error): ?>
                    <li><?= htmlspecialchars($error) ?></li>
                <?php endforeach; ?>
            </ul>
        </div>
    <?php endif; ?>

    <form method="post" action="<?= $task ? BASE_URL . '/tasks/edit/' . $task['id'] : BASE_URL . '/tasks/create/' . $project['id'] ?>">
        <label for="title">Título</label>
        <input type="text" id="title" name="title" required value="<?= htmlspecialchars($task['title'] ?? '') ?>">

        <label for="description">Descripción</label>
        <textarea id="description" name="description" rows="4"><?= htmlspecialchars($task['description'] ?? '') ?></textarea>

        <label for="due_date">Fecha límite</label>
        <input type="date" id="due_date" name="due_date" value="<?= htmlspecialchars($task['due_date'] ?? '') ?>">

        <?php if ($task): ?>
            <label for="status">Estado</label>
            <select id="status" name="status">
                <option value="pendiente" <?= $task['status'] === 'pendiente' ? 'selected' : '' ?>>Pendiente</option>
                <option value="en_progreso" <?= $task['status'] === 'en_progreso' ? 'selected' : '' ?>>En progreso</option>
                <option value="completada" <?= $task['status'] === 'completada' ? 'selected' : '' ?>>Completada</option>
            </select>
        <?php endif; ?>

        <div class="form-actions">
            <button type="submit" class="btn"><?= $task ? 'Guardar cambios' : 'Crear tarea' ?></button>
            <a href="<?= BASE_URL ?>/tasks/index/<?= $project['id'] ?>" class="btn btn-secondary">Cancelar</a>
        </div>
    </form>
</div>

<?php require APP_PATH . '/views/layout/footer.php'; ?>
