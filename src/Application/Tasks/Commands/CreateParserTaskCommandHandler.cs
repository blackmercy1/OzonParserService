using MediatR;
using OzonParserService.Domain.ParserTaskAggregate;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using Microsoft.Extensions.Logging;

namespace OzonParserService.Application.Tasks.Commands;

public class CreateParserTaskCommandHandler 
    : IRequestHandler<CreateParserTaskCommand, ErrorOr<ParserTask>>
{
    private readonly IBrowserFactory _browserFactory;
    private readonly IParserTaskRepository _taskRepository;
    private readonly ILogger<CreateParserTaskCommandHandler> _logger;

    public CreateParserTaskCommandHandler(
        IBrowserFactory browserFactory,
        IParserTaskRepository taskRepository,
        ILogger<CreateParserTaskCommandHandler> logger)
    {
        _browserFactory = browserFactory;
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<ParserTask>> Handle(
        CreateParserTaskCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var task = ParserTask.Create(
                request.ProductUrl,
                request.CheckInterval);
            
            var (price, error) = await ParseProductPrice(task.ProductUrl);
            
            if (!string.IsNullOrEmpty(error))
                return Error.Failure(description: error);
            
            task.Complete(price);
            await _taskRepository.AddAsync(task);

            return task;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating parser task");
            return Error.Unexpected(description: ex.Message);
        }
    }

    private async Task<(decimal? price, string error)> ParseProductPrice(string url)
    {
        await using var browser = await _browserFactory.CreateBrowserAsync();
        await using var page = await browser.NewPageAsync();
        
        try
        {
            // Настройка запросов
            await page.SetRequestInterceptionAsync(true);
            page.Request += (_, e) => 
            {
                if (e.Request.ResourceType == ResourceType.Image || 
                    e.Request.ResourceType == ResourceType.Stylesheet)
                    e.Request.AbortAsync();
                else
                    e.Request.ContinueAsync();
            };

            // Навигация
            var response = await page.GoToAsync(url);
            if (!response.Ok)
                return (null, $"HTTP Error: {response.Status}");

            // Извлечение данных
            var priceText = await page.EvaluateExpressionAsync<string>(
                "document.querySelector('[data-test-id='price']').innerText");
            
            if (decimal.TryParse(priceText.Replace(" ", ""), out var price))
                return (price, null);

            return (null, "Price format error");
        }
        catch (NavigationException ex)
        {
            return (null, $"Navigation failed: {ex.Message}");
        }
        catch (SelectorException ex)
        {
            return (null, $"Element not found: {ex.Message}");
        }
    }
}

// Вспомогательный класс для работы с браузером
public interface IBrowserFactory
{
    Task<IBrowser> CreateBrowserAsync();
}

public class PuppeteerBrowserFactory : IBrowserFactory
{
    private readonly LaunchOptions _options;

    public PuppeteerBrowserFactory(IConfiguration config)
    {
        _options = new()
        {
            Headless = true,
            Args = ["--no-sandbox"],
            ExecutablePath = config["Puppeteer:ChromePath"]
        };
    }

    public async Task<IBrowser> CreateBrowserAsync() => await Puppeteer.LaunchAsync(_options);
}
