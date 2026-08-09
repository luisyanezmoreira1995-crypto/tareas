<?php

class Project
{
    private PDO $db;

    public function __construct()
    {
        $this->db = Database::getInstance()->getConnection();
    }

    public function getAllByUser(int $userId): array
    {
        $stmt = $this->db->prepare(
            'SELECT p.*,
                (SELECT COUNT(*) FROM tasks t WHERE t.project_id = p.id) AS task_count,
                (SELECT COUNT(*) FROM tasks t WHERE t.project_id = p.id AND t.status = "completada") AS done_count
             FROM projects p
             WHERE p.user_id = :user_id
             ORDER BY p.created_at DESC'
        );
        $stmt->execute(['user_id' => $userId]);

        return $stmt->fetchAll();
    }

    public function find(int $id): ?array
    {
        $stmt = $this->db->prepare('SELECT * FROM projects WHERE id = :id');
        $stmt->execute(['id' => $id]);
        $project = $stmt->fetch();

        return $project ?: null;
    }

    public function create(int $userId, string $name, string $description): bool
    {
        $stmt = $this->db->prepare(
            'INSERT INTO projects (user_id, name, description) VALUES (:user_id, :name, :description)'
        );

        return $stmt->execute([
            'user_id' => $userId,
            'name' => $name,
            'description' => $description,
        ]);
    }

    public function update(int $id, string $name, string $description): bool
    {
        $stmt = $this->db->prepare(
            'UPDATE projects SET name = :name, description = :description WHERE id = :id'
        );

        return $stmt->execute([
            'name' => $name,
            'description' => $description,
            'id' => $id,
        ]);
    }

    public function delete(int $id): bool
    {
        $stmt = $this->db->prepare('DELETE FROM projects WHERE id = :id');

        return $stmt->execute(['id' => $id]);
    }
}
