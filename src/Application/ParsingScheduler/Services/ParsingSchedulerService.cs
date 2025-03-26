namespace OzonParserService.Application.ParsingScheduler.Services;

public class ParsingScheduler
    // : BackgroundService
{
    // protected override async Task ExecuteAsync(
    //     CancellationToken stoppingToken)
    // {
    //     while (!stoppingToken.IsCancellationRequested)
    //     {
    //         var dueTasks = await _repository.GetDueTasksAsync(); // 1. 
    //
    //         foreach (var task in dueTasks) 
    //             await ProcessTask(task);
    //
    //         await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
    //     }
    // }
    //
    // private async Task ProcessTask(
    //     ParserTask task)
    // {
    //     try
    //     {
    //         // 3. Запуск парсера
    //         var productData = await _scraper.ParseProductPage(task.ProductUrl);
    //
    //         // 4. Обновление ProductData
    //         var existingProduct = await _productRepo.GetByUrl(task.ProductUrl);
    //
    //         if (existingProduct == null)
    //             await _productRepo.AddAsync(productData);
    //         else
    //         {
    //             existingProduct.UpdateFrom(productData);
    //             await _productRepo.UpdateAsync(existingProduct);
    //         }
    //
    //         // 5. Создание PriceSnapshot
    //         await _priceRepo.AddAsync(new PriceSnapshot
    //         {
    //             ProductExternalId = productData.ExternalId,
    //             Price = productData.CurrentPrice,
    //             SnapshotDate = DateTime.UtcNow
    //         });
    //
    //         // 6. Отправка в брокер сообщений
    //         _messagePublisher.PublishProductData(productData);
    //
    //         // 7. Обновление задачи
    //         task.UpdateLastRunTime(DateTime.Now);
    //         await _taskRepo.UpdateAsync(task);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, $"Failed to process task {task.Id}");
    //     }
    // }
}
