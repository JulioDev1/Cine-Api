namespace CineApi.Dto
{
    public class CreateRequestMovie
    {
        public required MovieDto MovieDto { get; set; }
        public int Qtd { get; set; }

    }
}
