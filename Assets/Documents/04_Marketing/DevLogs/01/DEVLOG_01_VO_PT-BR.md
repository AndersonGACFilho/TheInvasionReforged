# Devlog #01 — Roteiro de Gravação VO (Português - pt-BR)

Use este arquivo para gravar a versão em Português do VO. Cada seção foi dividida para facilitar a gravação e edição.

0:00–0:08 Gancho (8s)
"Movimento em gravidade zero ativado — e finalmente está com a sensação certa."

0:08–0:25 Introdução (17s)
"Olá, eu sou [Seu Nome], desenvolvedor solo de The Invasion Reforged. Bem-vindo ao Devlog número um. Hoje vou ajustar o movimento da nave e as armas de disparo automático."

0:25–0:45 Objetivo & Contexto (20s)
"Objetivo desta sessão: adicionar inércia para uma sensação flutuante, mas responsiva, e fazer as armas automáticas funcionarem de maneira que recompense posicionamento e escolhas de upgrades."

0:45–2:10 Deep Dive do Movimento (grave em linhas curtas)
"Modelei o movimento como um sistema de velocidade amortecida."
"Ao invés de definir a velocidade diretamente, a nave acelera em direção à entrada e a velocidade existente é amortecida a cada frame."
"Principais parâmetros: curva de aceleração, fator de amortecimento e limite de velocidade máxima."
"Usei uma curva de aceleração não-linear para que pequenos inputs sejam ágeis, enquanto um input sustentado atinge a velocidade máxima de forma suave."

2:10–2:45 Explicação da Habilidade Dash (35s)
"Para complementar a inércia, adicionei uma dash curta com tempo de recarga."
"Isso preserva o risco — você não pode usar a dash sem limite — mas dá ao jogador uma opção de emergência para evitar interceptores kamikaze."

2:45–3:45 Armas de Tiro Automático (1:00)
"As armas disparam automaticamente com base no seu loadout e nível."
"O sistema escolhe alvos dentro de um cone e dispara conforme as taxas específicas de cada arma."
"É orientado por dados: novas armas são definidas por ScriptableObjects ou JSON que especificam taxa de disparo, ângulo do cone, tipo de projétil e escala com o nível."

3:45–4:10 Interação com Upgrades (25s)
"Quando você sobe de nível, upgrades temporários podem mudar o comportamento das suas armas automáticas — por exemplo, adicionando homing ou criando um orbitador."
"Essas mudanças temporárias se combinam com os upgrades permanentes do Hangar."

4:10–4:40 Problemas & Correções (30s)
"Na primeira versão, o movimento ficou lento."
"Aumentei a responsividade ajustando a curva de aceleração e adicionei a dash para melhorar a agência do jogador."
"Para as armas, ajustei o spread dos projéteis e adicionei escala por nível para evitar picos de poder no início."

4:40–5:10 Trecho de Jogabilidade (30s)
(Comentário ao vivo opcional — mantenha curto)
"Aqui está um run onde as pequenas correções importam — veja o Raven-IX se aproximando e a dash que salva a nave."

5:10–5:40 Próximos Passos & Roadmap (30s)
"Próximo: IA do Raven-IX, ajuste de spawn de níveis e o Hangar com upgrades permanentes."
"O Devlog número dois vai focar em IA inimiga e design de spawns."

5:40–6:00 Encerramento & CTA (20s)
"Se gostou, adicione The Invasion Reforged à sua wishlist e inscreva-se para devlogs semanais."
"Devlog completo e notas de código estão na descrição. Obrigado por assistir!"

---

Dicas de gravação (curtas)
- Grave linhas curtas separadamente e deixe 1–2 segundos de silêncio antes e depois de cada linha para facilitar a edição.
- Mantenha posição de microfone consistente, grave a 48 kHz, 24-bit se possível.
- Salve as tomadas brutas com nomes: Devlog01_PT_take01.wav

Substitua os placeholders antes de publicar: <STEAM_WISHLIST>, <DISCORD_INVITE>, <FULL_DEVLOG_URL>. 
