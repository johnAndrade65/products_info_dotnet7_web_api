using Microsoft.AspNetCore.Mvc;
using ProductsInfo;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class InfoProductsController : ControllerBase
{
    private readonly ProductsDbContext _context;

    public InfoProductsController(ProductsDbContext context)
    {
        _context = context;
    }

    [HttpGet("obter-todos-itens")] //Obter todos os dados armazenados
    public async Task<ActionResult<List<InfoProducts>>> ObterDadosProdutos(
        ProductsDbContext context
    )
    {
        List<InfoProducts> todoProductsInfo = await context.InfoProducts.ToListAsync();

        return Ok(todoProductsInfo);
    }

    [HttpGet("obter-todos-itens-com-arquivo")] //Obter todos os dados armazenados
    public async Task<ActionResult<List<InfoProducts>>> ObterDadosProdutosComArquivo(
        ProductsDbContext context
    )
    {
        List<InfoProductWithFile> ProductsInfoWithFiles =
            await context.InfoProductWithFile.ToListAsync();

        return Ok(ProductsInfoWithFiles);
    }

    [HttpGet("obter-todas-informacoes-de-produtos")]
    public async Task<ActionResult<List<InfoAllProducts>>> ObterDadosDeTodosProdutos(
        ProductsDbContext context
    )
    {
        var productsInfo = await context.InfoProducts.ToListAsync();
        var productsWithFile = await context.InfoProductWithFile.ToListAsync();

        var combinedData = productsWithFile
            .Select(
                info =>
                    new InfoAllProducts
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Description = info.Description,
                        FileUrl = info.FileUrl // Preencha aqui com o nome do arquivo se aplicável
                    }
            )
            .ToList();

        //Mapeie e adicione os dados de productsWithFile conforme necessário
        if (productsInfo == null && productsWithFile == null)
        {
            return NotFound("Os dados não foram encontrados!.");
        }

        return Ok(combinedData);
    }

    [HttpGet("{id}")] //Obter dados especificos pelo Id
    public async Task<ActionResult<InfoProducts>> ObterDadosProdutosPeloId(
        ProductsDbContext context,
        int id
    )
    {
        //Conectar ao banco de dados ou obter uma instância do contexto de banco de dados.
        //Consultar o banco de dados para obter o DadosContato com o ID especificado.
        InfoProducts todoProductsInfo = await context.InfoProducts.FindAsync(id);

        //Verificar se o DadosContato foi encontrado no banco de dados.
        if (todoProductsInfo == null)
        {
            //Se não foi encontrado, retornar um resultado NotFound (HTTP 404).
            return NotFound();
        }

        //Se o DadosContato foi encontrado, retornar um resultado Ok (HTTP 200) com os dados do contato.
        return Ok(todoProductsInfo);
    }

    [HttpPost("dados-produtos-sem-arquivo")]
    public async Task<ActionResult<InfoProducts>> PostProductsInfo(InfoProducts infoProducts)
    {
        try
        {
            _context.InfoProducts.Add(infoProducts);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Erro interno ao inserir os dados do produto.");
        }
        return CreatedAtAction(
            nameof(PostProductsInfo),
            new { id = infoProducts.Id },
            infoProducts
        );
    }

    [HttpPost("dados-produtos-com-arquivo")]
    public async Task<ActionResult<InfoProductWithFile>> PostProductsInfoComArquivo(
        InfoProductWithFile infoProducts
    )
    {
        _context.InfoProductWithFile.Add(infoProducts);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(PostProductsInfo),
            new { id = infoProducts.Id },
            infoProducts
        );
    }

    [HttpPut("dados-a-serem-atualizados")]
    public async Task<ActionResult<InfoProducts>> UpdateInfoProducts(
        InfoProducts infoProducts,
        int id
    )
    {
        InfoProducts todoProductsInfo = await _context.InfoProducts.FindAsync(id);

        if (todoProductsInfo == null)
        {
            return NotFound("Os dados do produto não foram encontrados.");
        }

        if (infoProducts.Id != id)
        {
            return BadRequest("Os dados do produto não correspondem.");
        }

        todoProductsInfo.Name = infoProducts.Name;
        todoProductsInfo.Description = infoProducts.Description;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(todoProductsInfo);
        }
        catch (DbUpdateException)
        {
            return StatusCode(
                500,
                "Erro interno ao tentar atualizar os dados armazenados no banco de dados"
            );
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<InfoProducts>> DeleteInfoProducts(int id)
    {
        InfoProducts todoProductsInfo = await _context.InfoProducts.FindAsync(id);

        if (id == null)
        {
            return NotFound("Id não encontrado, por favor verifique o Id e tente novamente!.");
        }
        else if (todoProductsInfo == null)
        {
            return NotFound(
                "Dados não encontrados, por favor verifique o Id inserido e tente novamente!."
            );
        }
        else if (todoProductsInfo != null)
        {
            try
            {
                _context.InfoProducts.Remove(todoProductsInfo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro interno ao atualizar os dados do produto.");
            }
        }

        return Ok(
            $"Dados do id:{id} foram deletados com sucesso!.\n O:{todoProductsInfo.Name} foi removido do banco de dados!."
        );
    }
}
