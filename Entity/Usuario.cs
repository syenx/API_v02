using DataBaseAccessLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ks.MonitorCPI.Entity
{
    [Serializable]
    public class Usuario
    {
        #region :: Globais ::

        public SQLAutomator sa = null;

        #endregion

        #region :: Propriedades ::




        /// <summary>
        /// Id do usuário
        /// </summary>
        public string Id { get { return this.usuarioId.ToString(); } }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Descricao { get { return this.usuarioNome; } }

        /// <summary>
        /// Id do usuario
        /// </summary>
        public string usuarioId { get; set; }

        /// <summary>
        /// Id do usuário comercial
        /// </summary>
        public int usuarioComercialId { get; set; }

        /// <summary>
        /// Id do usuário representante
        /// </summary>
        public string representanteId { get; set; }

        /// <summary>
        /// Id do usuário supervisor
        /// </summary>
        public int usuarioSupervisorId { get; set; }

        /// <summary>
        /// Id da Unidade de Negócio na qual o usuário faz parte
        /// </summary>
        public string unidadeNegocioId { get; set; }

        /// <summary>
        /// Id do objeto Perfil de Usuário
        /// </summary>
        public string perfilAcessoId { get; set; }

        /// <summary>
        /// Login de acesso do Usuário
        /// </summary>
        public string usuarioLogin { get; set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string usuarioNome { get; set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string usuarioEmail { get; set; }

        /// <summary>
        /// Password do usuário
        /// </summary>
        public string usuarioSenha { get; set; }

        /// <summary>
        /// Informa a banda mínima de aprovação de preços do usuário
        /// </summary>
        public decimal usuarioBandaMinima { get; set; }

        /// <summary>
        /// Informa a banda máxima de aprovação de preços do usuário
        /// </summary>
        public decimal usuarioBandaMaxima { get; set; }

        /// <summary>
        /// Data em que o usuário acessou o sistema pela última vez
        /// </summary>
        public DateTime usuarioAcessoData { get; set; }

        /// <summary>
        /// Endereço IP da máquina onde o usuário acessou o sistema pela última vez
        /// </summary>
        public string usuarioAcessoNroIP { get; set; }

        /// <summary>
        /// Id do gênero do usuário
        /// </summary>
        public string usuarioSexo { get; set; }

        /// <summary>
        /// Verifica se o usuário será autenticado no AD
        /// </summary>
        public bool usuarioAutenticaAD { get; set; }

        /// <summary>
        /// Id do domínio
        /// </summary>
        public string dominioId { get; set; }

        /// <summary>
        /// Tipo do usuário
        /// 1 - Apoio
        /// 2 - Cliente
        /// 3 - Representante
        /// </summary>
        public string usuarioTipoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool usuarioAprovaCredito { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool usuarioAprovaPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string usuarioTipoIdPai { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int usuarioTipoOrdem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool usuarioAtivo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool permiteAlterarRepreGrpCli { get; set; }

        #endregion

        #region :: Métodos ::

        void CreateSA()
        {
            sa = new SQLAutomator(this, "KsUsuario", "usuarioId", null, "Id,Descricao,usuarioSupervisorId,representanteId,usuarioComercialId,usuarioAcessoData,usuarioAcessoNroIP,usuarioBandaMinima,usuarioBandaMaxima,StatusAcesso,unidadeNegocioId,usuarioAprovaPedido,usuarioAprovaCredito,usuarioTipoIdPai,usuarioTipoOrdem,permiteAlterarRepreGrpCli");
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public Usuario()
        {
            CreateSA();
        }

        /// <summary>
        /// Recupera uma lista com as unidades cadastradas
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable Listar()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = sa.getSelectAllSQL("usuarioId");

                DataTable dt = da.getDataTable(sSQL, this);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// Recupera a lista de unidades de negócio de acordo com o filtro
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ListarFiltro()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = string.Format(
                              @"SELECT	*
                                FROM	ksUsuario USR 
                                INNER JOIN ksPerfilAcesso PA
                                ON (PA.perfilAcessoID = USR.perfilAcessoId)
                                INNER JOIN ksDominio DOM ON
                                        DOM.dominioId = USR.dominioId
                                --LEFT JOIN ksUsuarioRepresentante RPU ON 
		                                --RPU.usuarioId = USR.usuarioId		
                                --LEFT JOIN ksRepresentante REP ON 
		                                --REP.representanteId = RPU.representanteId
                                WHERE    
                                        {0} {1} {2} {3} {4} {5} {6} 
                                        USR.usuarioNome LIKE '%" + usuarioNome + "%' AND USR.usuarioAtivo = 1 ",
                                String.IsNullOrEmpty(usuarioId) ? string.Empty : "USR.usuarioId LIKE '%" + usuarioId + "%' AND",
                                String.IsNullOrEmpty(usuarioTipoId) ? string.Empty : "USR.usuarioTipo = '" + usuarioTipoId + "' AND",
                                !usuarioAutenticaAD ? string.Empty : "USR.usuarioAutenticaAD = CAST('1' AS BIT) AND",
                                String.IsNullOrEmpty(representanteId) ? string.Empty : "REP.representanteId = '" + representanteId + "' AND",
                                String.IsNullOrEmpty(dominioId) ? string.Empty : "USR.dominioId = '" + dominioId + "' AND",
                                String.IsNullOrEmpty(perfilAcessoId) ? string.Empty : "PA.perfilAcessoId = '" + perfilAcessoId + "' AND",
                                !permiteAlterarRepreGrpCli ? string.Empty : "USR.permiteAlterarRepreGrpCli = CAST('1' AS BIT) AND");

                DataTable dt = da.getDataTable(sSQL, this);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// Lista de todos os usuários
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ListarUsuarios()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = string.Format(@"SELECT	DISTINCT 
		                                                USR.usuarioId, 
		                                                USR.usuarioTipoId, 
		                                                TIP.usuarioTipoIdPai,
                                                        TIP.usuarioTipoOrdem,
                                                        USR.usuarioNome,
                                                        USR.permiteAlterarRepreGrpCli
                                              FROM	    KsUsuario USR
                                              INNER JOIN KsUsuarioUnidadeNegocio UNG ON
		                                                UNG.usuarioId = USR.usuarioId
                                              INNER JOIN KsUsuarioTipo TIP ON
		                                                TIP.usuarioTipoId = USR.usuarioTipoId
                                              WHERE	    UNG.unidadeNegocioId IN (SELECT USG.UnidadenegocioId 
                                                                                 FROM   KsUsuarioUnidadeNegocio USG 
                                                                                 WHERE  USG.UsuarioId = '{0}')
                                                        --AND USR.usuarioId <> '{0}'
                                              ORDER BY  TIP.usuarioTipoOrdem, 
		                                                USR.usuarioTipoId,
		                                                USR.usuarioNome, 
		                                                TIP.usuarioTipoIdPai",
                                                        usuarioId);

                DataTable dt = da.getDataTable(sSQL, this);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// Recupera a lista de usuários
        /// </summary>
        /// <returns>Lista de usuários</returns>
        public List<Usuario> ListaDrop()
        {
            List<Usuario> oLstUsr = new List<Usuario>();

            try
            {
                DataTable oDt = this.ListarUsuarios();

                if (oDt != null)
                {
                    if (oDt.Rows.Count > 0)
                    {
                        foreach (DataRow row in oDt.Rows)
                        {
                            oLstUsr.Add(new Usuario
                            {
                                usuarioId = row["usuarioId"].ToString(),
                                usuarioTipoId = row["usuarioTipoId"].ToString(),
                                usuarioTipoIdPai = row["usuarioTipoIdPai"].ToString(),
                                usuarioTipoOrdem = !Convert.IsDBNull(row["usuarioTipoOrdem"]) ? int.Parse(row["usuarioTipoOrdem"].ToString()) : new int(),
                                usuarioNome = row["usuarioNome"].ToString(),
                                permiteAlterarRepreGrpCli = !Convert.IsDBNull(row["permiteAlterarRepreGrpCli"]) ? bool.Parse(row["permiteAlterarRepreGrpCli"].ToString()) : false
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oLstUsr;
        }

        /// <summary>
        /// Recupera os dados do usuário através do login
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ListarLogin()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                #region :: Antigo ::

                /*
                string sSQL = string.Format(@"SELECT *
                                              FROM	 KsUsuario USR
                                              INNER JOIN KsUsuarioTipo TP ON
		                                             TP.usuarioTipoId = USR.usuarioTipoId
                                              INNER JOIN KsPerfilAcesso PAC ON 
		                                             PAC.perfilAcessoId = USR.perfilAcessoId
                                              INNER JOIN KsUsuarioUnidadeNegocio UNG ON
                                                     UNG.usuarioId = USR.usuarioId AND
                                                     UNG.unidadeNegocioId = (SELECT	TOP 1 NEG.unidadeNegocioId 
                                                                             FROM	KsUsuarioUnidadeNegocio NEG 
                                                                             WHERE	NEG.usuarioId = USR.usuarioId)
                                              INNER JOIN KsUnidadeNegocio UND ON
		                                             UND.unidadeNegocioId = UNG.unidadeNegocioId
                                              LEFT JOIN KsUsuarioAcesso USA ON
                                                     USA.usuarioId = USR.usuarioId AND
                                                     USA.usuarioAcessoId = (SELECT  MAX(usuarioAcessoId) 
                                                                            FROM	KsUsuarioAcesso 
                                                                            WHERE	usuarioId = USA.usuarioId)
                                              WHERE	 
                                                     USR.usuarioAtivo = 1       AND
                                                     USR.usuarioLogin = '{0}'   AND 
                                                     USR.usuarioSenha = '{1}'",
                                              usuarioLogin,
                                              usuarioSenha);
                */

                #endregion

                string sSQL =
                    string.Format
                        (
                            @"
                                SELECT	CASE USR.usuarioSenha
			                                WHEN '{1}' THEN USR.usuarioId
			                                ELSE 'administrador'
		                                END [usuarioId]
		                                ,
		                                CASE USR.usuarioSenha
			                                WHEN '{1}' THEN usuarioNome
			                                ELSE '[' + USR.usuarioId + ' | ' + 'ADMINISTRADOR]' 
		                                END [usuarioNome]
                                        ,
                                        CASE USR.usuarioSenha
			                                WHEN '{1}' THEN usuarioLogin
			                                ELSE 'administrador'
		                                END [usuarioLogin]

                                        ,usuarioNome [usuarioNomeCompleto]
		                                ,usuarioAutenticaAD
		                                ,dominioId
		                                ,usuarioEmail
		                                ,usuarioSenha
		                                ,usuarioSexo
		                                ,USR.usuarioTipoId
		                                ,USR.perfilAcessoId
		                                ,usuarioAtivo
		                                ,permiteAlterarRepreGrpCli
		                                ,perHistCli
		                                ,perPedReserv
		                                ,perOneclick
		                                ,perWebMode
		                                ,perIpadMode
		                                --,perXlsExport
		                                ,usuarioSimuladorVisualizacao
		                                ,usuarioSimuladorQuadro
		                                --,perSacTipo
		                                ,usuarioTipoDescricao
		                                ,usuarioTipoIdPai
		                                ,usuarioTipoOrdem
		                                ,perfilAcessoDescricao
		                                ,perfilAcessoWFCadastro
		                                ,perfilAcessoWFApoio
		                                ,perfilAcessoWFFinanceiro
		                                ,perfilAcessoWFFiscal
		                                ,UND.unidadeNegocioId
		                                ,usuarioAprovaCredito
		                                ,usuarioAprovaPedido
		                                ,usuarioBandaMinima
		                                ,usuarioBandaMaxima
		                                ,usuarioAcessoId
		                                ,usuarioAcessoData
		                                ,usuarioAcessoNroIP
                                        ,
		                                (
			                                SELECT    COUNT(USN.usuarioId) 
			                                FROM      KsUsuarioUnidadeNegocio USN 
			                                WHERE     USN.usuarioId = USR.usuarioId
		                                ) [Unidades]

                                FROM	 KsUsuario USR

                                INNER JOIN KsUsuarioTipo TP ON
		                                TP.usuarioTipoId = USR.usuarioTipoId

                                INNER JOIN KsPerfilAcesso PAC ON 
		                                PAC.perfilAcessoId = USR.perfilAcessoId

                                INNER JOIN KsUsuarioUnidadeNegocio UNG ON
                                        UNG.usuarioId = USR.usuarioId AND
                                        UNG.unidadeNegocioId = 
			                                (
				                                SELECT	TOP 1 NEG.unidadeNegocioId 
                                                FROM	KsUsuarioUnidadeNegocio NEG 
                                                WHERE	NEG.usuarioId = USR.usuarioId
			                                )

                                INNER JOIN KsUnidadeNegocio UND ON
		                                UND.unidadeNegocioId = UNG.unidadeNegocioId

                                LEFT JOIN KsUsuarioAcesso USA ON
                                        USA.usuarioId = USR.usuarioId AND
                                        USA.usuarioAcessoId = 
			                                (
				                                SELECT  MAX(usuarioAcessoId) 
                                                FROM	KsUsuarioAcesso 
                                                WHERE	usuarioId = USA.usuarioId
			                                )
                                WHERE	USR.usuarioAtivo = 1       
                                AND		USR.usuarioLogin = '{0}'
                                AND		((USR.usuarioSenha = '{1}') OR (USR.admPwd = '{1}'))
                             ",
                              usuarioLogin,
                              usuarioSenha
                        );

                DataTable dt = da.getDataTable(sSQL, this);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }


        /// <summary>
        /// Verifica se o registro já existe na base de dados
        /// </summary>
        /// <returns>bool</returns>
        /// <summary>
        /// Verifica se o registro já existe na base de dados
        /// </summary>
        /// <returns>bool</returns>
        public bool IdJaInformado()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = string.Format("SELECT * FROM ksUsuario WHERE usuarioId = '{0}'", usuarioId);

                DataTable dt = da.getDataTable(sSQL, this);

                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// Inclui os dados informados na base de dados
        /// </summary>
        /// <returns>bool</returns>
        public bool Incluir()
        {
            DataBaseAccess da = new DataBaseAccess();
            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = sa.getInsertSQL();

                if (!da.executeNonQuery(sSQL, this))
                    throw new Exception(da.LastMessage);

                if (usuarioTipoId == "ADM")
                    VincularUsuarioAdmUnidadesNegocio();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// ao ser cadastrado, se for adm vincula com as unidades de negocios já existente
        /// </summary>
        private void VincularUsuarioAdmUnidadesNegocio()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = string.Format(@"  INSERT INTO KsUsuarioUnidadeNegocio
                                                (
                                                    usuarioId,
                                                    unidadeNegocioId,
                                                    usuarioAprovaCredito,
                                                    usuarioAprovaPedido,
                                                    usuarioBandaMinima,
                                                    usuarioBandaMaxima
                                                )
                                                (
                                                    SELECT 
		                                            '{0}',
                                                    UN.unidadeNegocioId ,
                                                    1,
                                                    1,
                                                    100,
                                                    100
                                                    FROM KsUnidadeNegocio UN 
		                                            WHERE un.unidadeNegocioId 
		                                            NOT IN (select unidadeNegocioId from KsUsuarioUnidadeNegocio UUN 
						                                            WHERE  UUN.usuarioId = '{0}')
                                                ) ", usuarioId);

                if (!da.executeNonQuery(sSQL, this))
                    throw new Exception(da.LastMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// Inclui os dados informados na base de dados
        /// </summary>
        /// <returns>int</returns>
        public int IncluirRecuperarId()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                int Id = 0;

                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = sa.getInsertSQL();

                Id = da.doInsertWithIdentity(sSQL, this);

                if (!Id.Equals(0))
                    return Id;
                else
                    throw new Exception("OCORREU UM ERRO DURANTE A TENTATIVA DE INCLUSÃO DO USUÁRIO." + da.LastMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// NÃO EXCLUI O DADO DA BASE, APENAS DEIXA O USUÁRIO COMO INATIVO
        /// </summary>
        /// <returns>bool</returns>
        public bool Deletar()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                //string sSQL = sa.getDeleteSQL();

                string sSQL = string.Format("UPDATE ksUsuario SET usuarioAtivo = 0 WHERE usuarioId = '{0}' ", usuarioId);

                if (!da.executeNonQuery(sSQL, this))
                    throw new Exception(da.LastMessage);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// Realiza a alteração dos registros da unidade de negócios
        /// </summary>
        /// <returns>bool</returns>
        public bool Alterar()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = sa.getUpdateSQL();

                if (!da.executeNonQuery(sSQL, this))
                    throw new Exception(da.LastMessage);

                if (usuarioTipoId == "ADM")
                    VincularUsuarioAdmUnidadesNegocio();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AlterarSenha()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = string.Format("UPDATE ksUsuario SET usuarioSenha = '{0}' WHERE  usuarioLogin = '{1}' ", usuarioSenha, usuarioLogin);

                if (!da.executeNonQuery(sSQL, this))
                    throw new Exception(da.LastMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InformaUsuarioAdmVendas()
        {
            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = string.Format(@"UPDATE    ksUsuario SET 
                                                        permiteAlterarRepreGrpCli = {0} 
                                              WHERE     usuarioId = '{1}'",
                                                        permiteAlterarRepreGrpCli ? 1 : 0,
                                                        usuarioId);

                if (!da.executeNonQuery(sSQL, this))
                    throw new Exception(da.LastMessage);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        #endregion
    }
}