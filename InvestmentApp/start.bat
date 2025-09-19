@echo off
echo ====================================
echo INVESTMENT APP - SETUP E EXECUCAO
echo ====================================
echo.

echo 1. Restaurando pacotes NuGet...
dotnet restore
if %errorlevel% neq 0 (
    echo ERRO: Falha ao restaurar pacotes
    pause
    exit /b 1
)

echo.
echo 2. Compilando projeto...
dotnet build
if %errorlevel% neq 0 (
    echo ERRO: Falha na compilacao
    pause
    exit /b 1
)

echo.
echo 3. Verificando/Criando banco de dados...
dotnet ef database update
if %errorlevel% neq 0 (
    echo AVISO: Falha ao aplicar migrations (pode ser normal se o banco nao estiver acessivel)
)

echo.
echo 4. Iniciando aplicacao...
echo.
echo ==========================================
echo APLICACAO INICIADA COM SUCESSO!
echo ==========================================
echo.
echo 🌐 Swagger UI: https://localhost:5001
echo 🔗 API Base URL: https://localhost:5001/api
echo.
echo Credenciais de teste:
echo 👤 Admin: admin@investmentapp.com / admin123
echo 👤 User:  joao@teste.com / usuario123
echo.
echo Pressione Ctrl+C para parar a aplicacao
echo ==========================================
echo.

dotnet run
