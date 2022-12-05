from django.urls import path
from .views import ProfileView, DeleteProfileView

urlpatterns = [
    path('', ProfileView.as_view(), name='profile'),
    path('delete', DeleteProfileView.as_view(), name='delete_profile')
]
