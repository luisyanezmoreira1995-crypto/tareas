<?php
// Configuración de la base de datos (ajusta según tu entorno)
define('DB_HOST', 'localhost');
define('DB_NAME', 'gestion_tareas');
define('DB_USER', 'root');
define('DB_PASS', '');

// Ruta absoluta a la carpeta "app"
define('APP_PATH', dirname(__DIR__) . '/app');

// URL base detectada automáticamente a partir de dónde vive index.php.
// Funciona tanto si el document root apunta a /public como si el
// proyecto entero está dentro de htdocs y se accede vía el .htaccess raíz.
if (isset($_SERVER['SCRIPT_NAME'])) {
    $scriptDir = str_replace('\\', '/', dirname($_SERVER['SCRIPT_NAME']));
    define('BASE_URL', rtrim($scriptDir, '/'));
} else {
    define('BASE_URL', '');
}
