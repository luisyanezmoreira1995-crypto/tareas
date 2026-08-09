<?php

class Task
{
    private PDO $db;

    public function __construct()
    {
        $this->db = Database::getInstance()->getConnection();
    }

    public function getAllByProject(int $projectId): array
    {
        $stmt = $this->db->prepare(
            'SELECT * FROM tasks
             WHERE project_id = :project_id
             ORDER BY status = "completada", due_date IS NULL, due_date ASC, created_at DESC'
        );
        $stmt->execute(['project_id' => $projectId]);

        return $stmt->fetchAll();
    }

    public function find(int $id): ?array
    {
        $stmt = $this->db->prepare('SELECT * FROM tasks WHERE id = :id');
        $stmt->execute(['id' => $id]);
        $task = $stmt->fetch();

        return $task ?: null;
    }

    public function create(int $projectId, string $title, string $description, ?string $dueDate): bool
    {
        $stmt = $this->db->prepare(
            'INSERT INTO tasks (project_id, title, description, due_date)
             VALUES (:project_id, :title, :description, :due_date)'
        );

        return $stmt->execute([
            'project_id' => $projectId,
            'title' => $title,
            'description' => $description,
            'due_date' => $dueDate,
        ]);
    }

    public function update(int $id, string $title, string $description, ?string $dueDate, string $status): bool
    {
        $stmt = $this->db->prepare(
            'UPDATE tasks
             SET title = :title, description = :description, due_date = :due_date, status = :status
             WHERE id = :id'
        );

        return $stmt->execute([
            'title' => $title,
            'description' => $description,
            'due_date' => $dueDate,
            'status' => $status,
            'id' => $id,
        ]);
    }

    public function updateStatus(int $id, string $status): bool
    {
        $stmt = $this->db->prepare('UPDATE tasks SET status = :status WHERE id = :id');

        return $stmt->execute(['status' => $status, 'id' => $id]);
    }

    public function delete(int $id): bool
    {
        $stmt = $this->db->prepare('DELETE FROM tasks WHERE id = :id');

        return $stmt->execute(['id' => $id]);
    }
}
