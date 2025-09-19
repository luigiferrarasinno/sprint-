#!/usr/bin/env python3
"""
Arquivo de testes para a InvestmentApp API
Utiliza a biblioteca requests para testar todos os endpoints da API

Para executar:
1. Instale o requests: pip install requests
2. Execute a API: dotnet run
3. Execute este script: python tests_requests.py
"""

import requests
import json
from datetime import datetime, timezone

# Configuração base
BASE_URL = "http://localhost:5020/api"
# Desabilita warnings de SSL para ambiente de desenvolvimento (não necessário para HTTP)
# import urllib3
# urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning())

# Headers padrão
HEADERS = {
    "Content-Type": "application/json",
    "Accept": "application/json"
}

def print_response(response, description):
    """Imprime informações detalhadas da resposta"""
    print(f"\n{'='*60}")
    print(f"TESTE: {description}")
    print(f"{'='*60}")
    print(f"Status Code: {response.status_code}")
    print(f"URL: {response.url}")
    
    try:
        response_json = response.json()
        print(f"Response Body:\n{json.dumps(response_json, indent=2, ensure_ascii=False)}")
    except:
        print(f"Response Body (text): {response.text}")

def test_create_user():
    """Teste: Criar novo usuário"""
    user_data = {
        "nome": "Ana Paula Costa",
        "email": "ana.costa@email.com",
        "senha": "senha123456",
        "cpf": "12345678901",
        "dataNascimento": "1992-08-20T00:00:00Z",
        "role": "User"
    }
    
    response = requests.post(
        f"{BASE_URL}/users",
        headers=HEADERS,
        json=user_data
    )
    
    print_response(response, "Criar Novo Usuário")
    
    if response.status_code == 201:
        user = response.json()
        return user["id"]
    return None

def test_get_investments():
    """Teste: Listar todos os investimentos"""
    response = requests.get(
        f"{BASE_URL}/investimentos",
        headers=HEADERS
    )
    
    print_response(response, "Listar Todos os Investimentos")
    
    if response.status_code == 200:
        investments = response.json()
        return investments[0]["id"] if investments else None
    return None

def test_create_investment_admin():
    """Teste: Criar investimento (como Admin)"""
    # Primeiro, vamos tentar obter o ID do admin
    # Em um ambiente real, você obteria isso através de login
    admin_user_id = "00000000-0000-0000-0000-000000000001"  # ID fictício para demo
    
    investment_data = {
        "name": "LCI Banco ABC",
        "type": "Renda Fixa",
        "baseValue": 5000.00,
        "expectedYieldPercent": 9.80,
        "riskLevel": "Baixo",
        "description": "Letra de Crédito Imobiliário isenta de IR"
    }
    
    headers_with_auth = HEADERS.copy()
    headers_with_auth["userId"] = admin_user_id
    
    response = requests.post(
        f"{BASE_URL}/investimentos",
        headers=headers_with_auth,
        json=investment_data
    )
    
    print_response(response, "Criar Investimento (Admin)")
    
    if response.status_code == 201:
        investment = response.json()
        return investment["id"]
    return None

def test_create_user_investment(user_id, investment_id):
    """Teste: Criar investimento do usuário"""
    if not user_id or not investment_id:
        print("\n❌ PULANDO TESTE: IDs de usuário ou investimento não disponíveis")
        return
    
    user_investment_data = {
        "investmentId": investment_id,
        "amountInvested": 10000.00,
        "units": 100.0,
        "purchaseDate": "2024-01-15T10:30:00Z",
        "currentValue": 10500.00,
        "status": "Ativo"
    }
    
    headers_with_auth = HEADERS.copy()
    headers_with_auth["userId"] = user_id
    
    response = requests.post(
        f"{BASE_URL}/users/{user_id}/investimentos",
        headers=headers_with_auth,
        json=user_investment_data
    )
    
    print_response(response, "Criar Investimento do Usuário")
    
    if response.status_code == 201:
        user_investment = response.json()
        return user_investment["id"]
    return None

def test_get_user_investments(user_id):
    """Teste: Listar investimentos do usuário"""
    if not user_id:
        print("\n❌ PULANDO TESTE: ID de usuário não disponível")
        return
    
    headers_with_auth = HEADERS.copy()
    headers_with_auth["userId"] = user_id
    
    response = requests.get(
        f"{BASE_URL}/users/{user_id}/investimentos",
        headers=headers_with_auth
    )
    
    print_response(response, "Listar Investimentos do Usuário")

def test_get_specific_investment(investment_id):
    """Teste: Obter investimento específico"""
    if not investment_id:
        print("\n❌ PULANDO TESTE: ID de investimento não disponível")
        return
    
    response = requests.get(
        f"{BASE_URL}/investimentos/{investment_id}",
        headers=HEADERS
    )
    
    print_response(response, "Obter Investimento Específico")

def test_update_user_investment(user_id, user_investment_id):
    """Teste: Atualizar investimento do usuário"""
    if not user_id or not user_investment_id:
        print("\n❌ PULANDO TESTE: IDs não disponíveis")
        return
    
    update_data = {
        "amountInvested": 12000.00,
        "units": 120.0,
        "purchaseDate": "2024-01-15T10:30:00Z",
        "currentValue": 12600.00,
        "status": "Ativo",
        "isActive": True
    }
    
    headers_with_auth = HEADERS.copy()
    headers_with_auth["userId"] = user_id
    
    response = requests.put(
        f"{BASE_URL}/users/{user_id}/investimentos/{user_investment_id}",
        headers=headers_with_auth,
        json=update_data
    )
    
    print_response(response, "Atualizar Investimento do Usuário")

def test_unauthorized_access():
    """Teste: Tentativa de acesso não autorizado"""
    # Tentar listar usuários sem ser admin
    fake_user_id = "00000000-0000-0000-0000-000000000099"
    
    headers_with_fake_auth = HEADERS.copy()
    headers_with_fake_auth["userId"] = fake_user_id
    
    response = requests.get(
        f"{BASE_URL}/users",
        headers=headers_with_fake_auth
    )
    
    print_response(response, "Tentativa de Acesso Não Autorizado (Listar Usuários)")

def test_validation_errors():
    """Teste: Erros de validação"""
    # Tentar criar usuário com dados inválidos
    invalid_user_data = {
        "nome": "",  # Nome vazio
        "email": "email-invalido",  # Email inválido
        "senha": "123",  # Senha muito curta
        "cpf": "123",  # CPF inválido
        "dataNascimento": "data-invalida"
    }
    
    response = requests.post(
        f"{BASE_URL}/users",
        headers=HEADERS,
        json=invalid_user_data
    )
    
    print_response(response, "Erros de Validação (Dados Inválidos)")

def main():
    """Função principal que executa todos os testes"""
    print(f"🚀 INICIANDO TESTES DA INVESTMENT APP API")
    print(f"🌐 Base URL: {BASE_URL}")
    print(f"⏰ Data/Hora: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    
    try:
        # 1. Criar usuário
        user_id = test_create_user()
        
        # 2. Listar investimentos disponíveis
        investment_id = test_get_investments()
        
        # 3. Obter investimento específico
        test_get_specific_investment(investment_id)
        
        # 4. Criar investimento como admin (pode falhar se não for admin)
        new_investment_id = test_create_investment_admin()
        
        # 5. Criar investimento do usuário
        user_investment_id = test_create_user_investment(user_id, investment_id)
        
        # 6. Listar investimentos do usuário
        test_get_user_investments(user_id)
        
        # 7. Atualizar investimento do usuário
        test_update_user_investment(user_id, user_investment_id)
        
        # 8. Teste de acesso não autorizado
        test_unauthorized_access()
        
        # 9. Teste de erros de validação
        test_validation_errors()
        
        print(f"\n{'='*60}")
        print("✅ TESTES CONCLUÍDOS!")
        print("📋 Verifique os resultados acima para análise detalhada")
        print("🔍 Acesse o Swagger UI em: http://localhost:5020")
        print(f"{'='*60}")
        
    except requests.exceptions.ConnectionError:
        print("\n❌ ERRO: Não foi possível conectar à API")
        print("🔧 Verifique se a aplicação está rodando em: http://localhost:5020")
        print("💡 Execute: dotnet run")
    except Exception as e:
        print(f"\n❌ ERRO INESPERADO: {str(e)}")

if __name__ == "__main__":
    main()
