using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsInfo;

public class InfoProducts
{
    [Key]//Indica que a propriedade é a chave primária
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Informa ao EF que o banco de dados irá gerar o valor
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

}