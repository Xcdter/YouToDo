using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YouToDo.Models
{
    public class TaskProjectModel
    {
        public IEnumerable<TaskModel> Tasks { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public string FilteredPriority { get; set; }

        public short? FilteredPriorityValue { get; set; }

        public string ActiveTag { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int? ActiveProjectId { get; set; }

        public string GenerateUrl(IUrlHelper urlHelper, string action, string controller, object additionalRouteValues, bool resetFilters = false)
        {
            // Базовые параметры
            var currentRouteValues = new Dictionary<string, object>();

            if (!resetFilters)
            {
                // Сохраняем текущие фильтры, если сброс фильтров не требуется
                if (ActiveProjectId.HasValue)
                {
                    currentRouteValues["projectId"] = ActiveProjectId;
                }
                if (FilteredPriorityValue.HasValue)
                {
                    currentRouteValues["priority"] = FilteredPriorityValue;
                }
                if (!string.IsNullOrEmpty(ActiveTag))
                {
                    currentRouteValues["tag"] = ActiveTag;
                }
            }
            else
            {
                // При сбросе фильтров сохраняем только projectId
                if (ActiveProjectId.HasValue)
                {
                    currentRouteValues["projectId"] = ActiveProjectId;
                }
            }

            // Преобразуем дополнительные параметры в словарь
            var additionalValues = additionalRouteValues.GetType()
                .GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(additionalRouteValues));

            // Убираем параметр "page", если он равен 1 или общее количество страниц равно 1
            if (additionalValues.ContainsKey("page"))
            {
                int pageValue = Convert.ToInt32(additionalValues["page"]);
                if (pageValue == 1 || TotalPages == 1)
                {
                    additionalValues.Remove("page");
                }
            }

            // Объединяем текущие параметры с дополнительными, заменяя их при необходимости
            foreach (var kvp in additionalValues)
            {
                currentRouteValues[kvp.Key] = kvp.Value;
            }

            return urlHelper.Action(action, controller, currentRouteValues);
        }
    }
}
