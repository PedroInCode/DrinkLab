namespace DrinkLab.Model.Enums;

public enum TipoDocumento
{
    CPF,
    CNPJ
}

public enum TipoCargo
{
    Gerente,
    Caixa,
    Motorista
}

public enum StatusCompra
{
    Aberto,
    Fechado,
    Cancelado
}

public enum TipoCompra
{
    Presencial,
    Online
}

public enum FormaPagamento
{
    Dinheiro,
    Pix,
    Cartao
}

public enum StatusEntrega
{
    EmTransito,
    Atrasado,
    Entregue
}