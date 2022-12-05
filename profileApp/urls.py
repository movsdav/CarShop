from django.urls import path

from authApp.views import ProfileUpdateView
from .views import ProfileView, DeleteProfileView

urlpatterns = [
    path('', ProfileView.as_view(), name='profile'),
    path('edit', ProfileUpdateView.as_view(), name='edit_profile'),
    path('delete', DeleteProfileView.as_view(), name='delete_profile')
]
