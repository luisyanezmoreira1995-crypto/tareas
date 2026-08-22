<?php
// Configuración de la base de datos (ajusta según tu entorno)
define('DB_HOST', 'localhost');
define('DB_NAME', 'gestion_tareas');
define('DB_USER', 'root');
define('DB_PASS', '');

// Ruta absoluta a la carpeta "app"
define('APP_PATH', dirname(__DIR__) . '/app');

// Autoload sencillo para las clases del módulo de reportes (contratos,
// adaptador de PDF e implementaciones). Los controllers/models siguen
// cargándose como antes, vía require manual en App.php/Controller.php.
spl_autoload_register(function (string $class): void {
    $directories = [
        APP_PATH . '/contracts',
        APP_PATH . '/pdf',
        APP_PATH . '/reports',
    ];

    foreach ($directories as $directory) {
        $file = $directory . '/' . $class . '.php';

        if (file_exists($file)) {
            require_once $file;
            return;
        }
    }
});

// URL base detectada automáticamente a partir de dónde vive index.php.
// Funciona tanto si el document root apunta a /public como si el
// proyecto entero está dentro de htdocs y se accede vía el .htaccess raíz.
if (isset($_SERVER['SCRIPT_NAME'])) {
    $scriptDir = str_replace('\\', '/', dirname($_SERVER['SCRIPT_NAME']));
    define('BASE_URL', rtrim($scriptDir, '/'));
} else {
    define('BASE_URL', '');
}
