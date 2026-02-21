Plano: Tipos de Caixa Postal (Cortesia, Anuidade, Mensalidade)
Contexto
Atualmente o sistema tem apenas o campo ValorMensal e gera cobranças mensais para todas as caixas ativas. Precisamos diferenciar o comportamento por tipo de caixa:

Tipo	Valor	Cobrança	Comportamento
Mensalidade (3)	Valor Mensal	Mensal	Mantém como está hoje - cobra todo mês
Anuidade (1)	Valor Total (anual)	1x ao ano	Cobra uma vez por ano o valor total
Cortesia (2)	Sem valor	Nenhuma	Apenas existe no sistema, sem cobranças
Arquivos a modificar
1. Seed Data - TipoCaixa
Arquivo: Data/Configurations/TipoCaixaConfiguration.cs

Adicionar tipo Mensalidade (Id = 3) ao seed
Tipos finais: Anuidade (1), Cortesia (2), Mensalidade (3)
2. Enum - TipoCaixaEnum
Arquivo: Models/Enums/TipoCaixaEnum.cs

Adicionar Mensalidade = 3
3. Model - CaixaPostal
Arquivo: Models/CaixaPostal.cs

Renomear ValorMensal para Valor (campo genérico que serve para mensal ou anual)
Cortesia terá Valor = 0
4. Configuração EF - CaixaPostal
Arquivo: Data/Configurations/CaixaPostalConfiguration.cs

Renomear coluna ValorMensal para Valor
5. ViewModel - CaixaPostalViewModel
Arquivo: ViewModels/CaixaPostalViewModel.cs

Renomear ValorMensal para Valor
Remover [Required] e [Range] do valor (Cortesia não precisa)
Validação de valor será feita no controller conforme o tipo
6. Controller - CaixasPostaisController
Arquivo: Controllers/CaixasPostaisController.cs

Atualizar mapeamento ValorMensal → Valor
Adicionar validação no POST:
Se Cortesia: Valor = 0 (forçar)
Se Anuidade ou Mensalidade: valor obrigatório > 0
Default ao criar: IdTipoCaixa = Mensalidade (manter comportamento atual)
7. Controller - CobrancasController
Arquivo: Controllers/CobrancasController.cs

Método GerarCobrancasMensais - Alterar a lógica de geração:
Cortesia: Ignorar (não gera cobrança)
Mensalidade: Mantém como está (gera todo mês, verifica se já existe no mês/ano)
Anuidade: Gera 1x ao ano (verifica se já existe cobrança no ANO atual)
Renomear botão/mensagem para "Gerar Cobranças" (não só mensais)
8. Views CaixasPostais (Criar, Editar, Index, Detalhes)
Arquivos: Views/CaixasPostais/Criar.cshtml, Editar.cshtml, Index.cshtml, Detalhes.cshtml

Renomear label "Valor Mensal (R$)" para "Valor (R$)"
Adicionar JS para mostrar/esconder campo valor conforme tipo selecionado:
Cortesia: esconder campo valor
Anuidade: mostrar como "Valor Anual (R$)"
Mensalidade: mostrar como "Valor Mensal (R$)"
9. View Cobranças Index
Arquivo: Views/Cobrancas/Index.cshtml

Renomear botão "Gerar Cobranças do Mês" para "Gerar Cobranças"
10. Migration
Renomear coluna ValorMensal para Valor
Adicionar seed do tipo Mensalidade (Id=3)
Lógica de geração de cobranças (GerarCobrancas)

Para cada caixa ATIVA:
  Se tipo == Cortesia → pular

  Se tipo == Mensalidade:
    - Verificar se já existe cobrança no mês/ano atual
    - Se não: criar com Valor = caixa.Valor, vencimento = DiaVencimento do mês atual

  Se tipo == Anuidade:
    - Verificar se já existe cobrança no ANO atual
    - Se não: criar com Valor = caixa.Valor, vencimento = DiaVencimento do mês atual
Validação no cadastro de CaixaPostal

Se tipo == Cortesia:
  - Valor forçado para 0
  - DiaVencimento não é necessário (mas pode manter)

Se tipo == Anuidade ou Mensalidade:
  - Valor obrigatório e > 0
  - DiaVencimento obrigatório (1-31)
Verificação
Criar caixa tipo Cortesia → campo valor escondido, salva com valor 0
Criar caixa tipo Mensalidade → funciona como antes
Criar caixa tipo Anuidade → campo mostra "Valor Anual"
Gerar cobranças → Cortesia ignorada, Mensalidade gera mensal, Anuidade gera 1x/ano
Build sem erros, migration aplicada



Atualizar TipoCaixaEnum e seed data (adicionar Mensalidade)

Renomear ValorMensal para Valor no Model CaixaPostal

Atualizar configuração EF (coluna ValorMensal → Valor)

Atualizar ViewModel (ValorMensal → Valor, remover Required)

Atualizar CaixasPostaisController (mapeamento + validação por tipo)

Atualizar CobrancasController (lógica por tipo)

Atualizar Views CaixasPostais (Criar, Editar, Index, Detalhes)

Atualizar View Cobranças Index (botão)

Gerar e aplicar migration