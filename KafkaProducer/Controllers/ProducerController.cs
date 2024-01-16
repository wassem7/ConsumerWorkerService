using Confluent.Kafka;
using KafkaDotNet.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KafkaDotNet.Controllers;

[ApiController]
[Route("api/producer")]
public class ProducerController : ControllerBase
{
    [HttpPost("produce-order")]
    public async Task<IActionResult> ProduceMessage([FromBody] OrderRequest orderRequest)
    {
        try
        {
            var config = new ProducerConfig() { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var response = await producer.ProduceAsync(
                "orders",
                new Message<Null, string>() { Value = JsonConvert.SerializeObject(orderRequest) }
            );

            return Ok(response.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
