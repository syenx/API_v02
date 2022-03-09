using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Ks.MonitorCPI.Entidades;
using System;
using System.Collections.Generic;

namespace Ks.MonitorCPI.Models
{
    public class ViewModelTransactionLog
    {


        public string status { get; set; }
        public DateTime datefrom { get; set; }
        public string hourFrom { get; set; }
        public string tableid { get; set; }
        public ROOT ROOT { get; set; }



        public string[] listaDeTableId { get; set; }
        public string[] listaDeStatus { get; set; }



        public ViewModelTransactionLog()
        {
            listaDeTableId = tbItem;
            listaDeStatus = listaStatus;
        }

        string[] listaStatus = { "Erro" , "Sucesso", };
        string[] tbItem = { "Todos",
                                            "Medico__c"
                                        ,
                                            "NotaFiscal2__c"
                                        ,
                                            "NotaFiscal2Item__c"
                                        ,
                                            "AccountEndereco__c"
                                        ,
                                            "Estoque__c"
                                        ,
                                            "EstoqueKraft__c"
                                        ,
                                            "Pais__c"
                                        ,
                                            "UnidadeFederativa__c"
                                        ,
                                            "Cidade__c"
                                        ,
                                            "Empresa__c"
                                        ,
                                            "Loja__c"
                                        ,
                                            "FamiliaComercial__c"
                                        ,
                                            "CategoriaCliente__c"
                                        ,
                                            "ClassificacaoFiscal__c"
                                        ,
                                            "CondicaoPagamento__c"
                                        ,
                                            "GrupoComercial__c"
                                        ,
                                            "GrupoEstoque__c"
                                        ,
                                            "NaturezaOperacao__c"
                                        ,
                                            "UnidadeMedida__c"
                                        ,
                                            "TipoProduto__c"
                                        ,
                                            "GrupoComercialDeposito__c"
                                        ,
                                            "FamiliaMaterial__c"
                                        ,
                                            "Gerente__c"
                                        ,
                                            "Portador__c"
                                        ,
                                            "TipoPedido__c"
                                        ,
                                            "Fabricante__c"
                                        ,
                                            "Transportadora__c"
                                        ,
                                            "Representante__c"
                                        ,
                                            "CanalVenda__c"

                                            ,"Account"
                                            ,"Contact"

                                            ,"Product2"
                                            ,"Order"

                                         };

    }
}