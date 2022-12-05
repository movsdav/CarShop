from django.urls import path
from .views import IndexView, AddProductView, EditProductView, DeleteProductView

urlpatterns = [
    path('', IndexView.as_view(), name="products"),
    path('add_product/', AddProductView.as_view(), name="add_product"),
    path('edit_product/', EditProductView.as_view(), name="edit_product"),
    path('delete_product/', DeleteProductView.as_view(), name="delete_product"),
]