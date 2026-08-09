<?php require APP_PATH . '/views/layout/header.php'; ?>

<div class="page-header">
    <h1>Mis proyectos</h1>
    <a href="<?= BASE_URL ?>/projects/create" class="btn">+ Nuevo proyecto</a>
</div>

<?php if (empty($projects)): ?>
    <p class="muted">Todavía no tienes proyectos. Crea el primero para empezar a organizar tareas.</p>
<?php else: ?>
    <div class="grid">
        <?php foreach ($projects as $project): ?>
            <div class="card">
                <h2><?= htmlspecialchars($project['name']) ?></h2>
                <p class="muted"><?= nl2br(htmlspecialchars($project['description'])) ?></p>
                <p class="badge">
                    <?= (int) $project['done_count'] ?>/<?= (int) $project['task_count'] ?> tareas completadas
                </p>
                <div class="card-actions">
                    <a href="<?= BASE_URL ?>/tasks/index/<?= $project['id'] ?>" class="btn btn-sm">Ver tareas</a>
                    <a href="<?= BASE_URL ?>/projects/edit/<?= $project['id'] ?>" class="btn btn-sm btn-secondary">Editar</a>
                    <form method="post" action="<?= BASE_URL ?>/projects/delete/<?= $project['id'] ?>"
                          onsubmit="return confirm('¿Eliminar este proyecto y todas sus tareas?');">
                        <button type="submit" class="btn btn-sm btn-danger">Eliminar</button>
                    </form>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
<?php endif; ?>

<?php require APP_PATH . '/views/layout/footer.php'; ?>
