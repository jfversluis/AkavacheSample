using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Akavache;
using BeenPwned.Api;
using BeenPwned.Api.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace AkavacheSample
{
    public partial class AkavacheSamplePage : ContentPage
    {
        public bool IsLoading;

		private readonly ObservableRangeCollection<Breach> _breaches = new ObservableRangeCollection<Breach>();
		public ObservableCollection<Breach> Breaches => _breaches;

		private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                    (refreshCommand = new Command(() => LoadBreaches(), () => !IsLoading));
            }
        }

		private IBeenPwnedClient _pwnedClient = new BeenPwnedClient($"AkavacheSample");

		public AkavacheSamplePage()
        {
            InitializeComponent();

            BindingContext = this;

            RefreshCommand.Execute(null);
        }

		private void LoadBreaches()
		{
			IsLoading = true;

			GetBreaches(true);
			
            IsLoading = false;
		}

		public void GetBreaches(bool force = false)
		{
			var cache = BlobCache.LocalMachine;
			cache.GetAndFetchLatest("breaches", GetRemoteBreachesAsync,
				offset =>
				{
					TimeSpan elapsed = DateTimeOffset.Now - offset;
					var invalidateCache = (force || elapsed > new TimeSpan(24, 0, 0));
					return invalidateCache;
				})
				.Subscribe((cities) =>
				{
                    _breaches.ReplaceRange(cities);
				});
		}

		private async Task<IEnumerable<Breach>> GetRemoteBreachesAsync()
		{
            return await _pwnedClient.GetAllBreaches();
		}
    }
}