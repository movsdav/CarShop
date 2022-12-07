from django.urls import path
from .views import DeleteProductFromWatchList, IndexView, AddProductView, EditProductView, DeleteProductView, AddProductToWatchList

urlpatterns = [
    path('', IndexView.as_view(), name="products"),
    path('add_product/', AddProductView.as_view(), name="add_product"),
    path('edit_product/', EditProductView.as_view(), name="edit_product"),
    path('delete_product/', DeleteProductView.as_view(), name="delete_product"),
    path('add_product_to_watch_list', AddProductToWatchList.as_view(), name='add_to_watch_list'),
    path('delete_product_from_watch_list', DeleteProductFromWatchList.as_view(), name='delete_from_watch_list'),
]
