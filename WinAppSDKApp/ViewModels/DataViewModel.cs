using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using GoogleMapper.Core.Models;
using GoogleMapper.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GoogleMapper.ViewModels
{
    public class DataViewModel : ObservableObject
    {
        #region Objects and variables

        private readonly SqlServerDataService _svc = Ioc.Default.GetService<SqlServerDataService>();

        #endregion

        #region Properties

        /// <summary>
        /// The data source for this ViewModel.
        /// </summary>
        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        #endregion

        #region Public methods and functions

        public async Task LoadProjectsAsync()
        {
            Projects.Clear();

            List<Project> data = await _svc.GetProjectsAsync();
            data.ForEach(p => Projects.Add(p));
        }

        #endregion
    }
}