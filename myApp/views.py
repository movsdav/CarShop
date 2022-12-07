from django.shortcuts import render, redirect
from django.views import View
from django.forms.models import model_to_dict
from django.http.response import HttpResponse

from .models import Car
from .forms import ManipulateProductForm

from authApp.models import UserProfile


# Create your views here.
class IndexView(View):
    template_name = 'myApp/index.html'
    context = {

    }

    def get(self, req, *args, **kwargs):
        cars = Car.objects.all()

        self.context['cars'] = cars

        return render(req, self.template_name, self.context)


class AddProductView(View):
    action = 'Add'
    template_name = 'myApp/manipulate_product.html'
    context = {

    }
    form_class = ManipulateProductForm

    def get(self, req, *args, **kwargs):
        form = self.form_class(req.POST or None, action=self.action)
        self.context['action'] = self.action
        self.context['form'] = form
        return render(req, self.template_name, self.context)

    def post(self, req, *args, **kwargs):
        form = self.form_class(data=req.POST or None, files=req.FILES)
        print('===========================================')
        print(form.is_valid())
        print('===========================================')
        if form.is_valid():
            form.save()

        return redirect('products', permanent=True)


class EditProductView(View):
    action = 'Edit'
    template_name = 'myApp/manipulate_product.html'
    context = {

    }
    form_class = ManipulateProductForm

    def get(self, req, *args, **kwargs):
        car_id = req.GET.get('id')
        car = Car.objects.get(id=car_id)

        form = self.form_class(initial=model_to_dict(car), action=self.action)

        self.context['action'] = self.action
        self.context['form'] = form
        return render(req, self.template_name, self.context)

    def post(self, req, *args, **kwargs):
        car_id = req.GET.get('id', None)
        if car_id is not None:
            car = Car.objects.get(id=car_id)
        else:
            return redirect('products')
        form = self.form_class(req.POST or None, files=req.FILES, instance=car, action=self.action)

        if form.is_valid():
            form.save()

        return redirect('products', permanent=True)


class DeleteProductView(View):
    def get(self, req, *args, **kwargs):
        car_id = req.GET.get('id')
        Car.objects.get(id=car_id).delete()
        return redirect('products', permanent=True)


class AddProductToWatchList(View):
    def get(self, req):
        product_id = req.GET.get('id')
        product = Car.objects.get(id=product_id)

        user = req.user
        profile = UserProfile.objects.get(user=user)
        profile.watch_list.add(product)
        return redirect('products')


class DeleteProductFromWatchList(View):
    def get(self, req):
        product_id = req.GET.get('id')
        product = Car.objects.get(id=product_id)

        profile = UserProfile.objects.get(user=req.user)
        profile.watch_list.remove(product)
        return redirect('profile')

