angular.module('starter.controllers', [])
  .value("PaymentData", {})
  .controller('AppCtrl', function ($scope, $ionicModal, $timeout) {

    // With the new view caching in Ionic, Controllers are only called
    // when they are recreated or on app start, instead of every page change.
    // To listen for when this page is active (for example, to refresh data),
    // listen for the $ionicView.enter event:
    //$scope.$on('$ionicView.enter', function(e) {
    //});

    // Form data for the login modal
    $scope.loginData = {};

    // Create the login modal that we will use later
    $ionicModal.fromTemplateUrl('templates/login.html', {
      scope: $scope
    }).then(function (modal) {
      $scope.modal = modal;
    });

    // Triggered in the login modal to close it
    $scope.closeLogin = function () {
      $scope.modal.hide();
    };

    // Open the login modal
    $scope.login = function () {
      $scope.modal.show();
    };

    // Perform the login action when the user submits the login form
    $scope.doLogin = function () {
      console.log('Doing login', $scope.loginData);

      // Simulate a login delay. Remove this and replace with your login
      // code if using a login system
      $timeout(function () {
        $scope.closeLogin();
      }, 1000);
    };
  })

  .controller('PlaylistsCtrl', function ($scope, $ionicBackdrop, $ionicModal, $ionicSlideBoxDelegate, $ionicScrollDelegate) {
    $scope.allImages = [];
    $scope.zoomMin = 1;


    $scope.loadImages = function () {
      var imgs = [];
      for (var j = 0; j < 5; j++) {
        //let x = Math.floor((Math.random() * 500) + 200);
        //let y = Math.floor((Math.random() * 500) + 200);
        imgs.push({ src: 'https://picsum.photos/' + 500 + '/' + 400 + '?random=' + j.toString() });
        imgs.push({ src: 'https://picsum.photos/' + 600 + '/' + 400 + '?random=' + j.toString() });
        imgs.push({ src: 'https://picsum.photos/' + 550 + '/' + 300 + '?random=' + j.toString() });
        imgs.push({ src: 'https://picsum.photos/' + 750 + '/' + 500 + '?random=' + j.toString() });
        imgs.push({ src: 'https://picsum.photos/' + 550 + '/' + 500 + '?random=' + j.toString() });
      }
      $scope.allImages = imgs;
    };
    $scope.loadImages();

    $scope.showImages = function (index) {
      $scope.activeSlide = index;
      $scope.showModal('templates/gallery-zoomview.html');
    };

    $scope.showModal = function (templateUrl) {
      $ionicModal.fromTemplateUrl(templateUrl, {
        scope: $scope
      }).then(function (modal) {
        $scope.modal = modal;
        $scope.modal.show();
      });
    }

    $scope.closeModal = function () {
      $scope.modal.hide();
      $scope.modal.remove()
    };

    $scope.updateSlideStatus = function (slide) {
      var zoomFactor = $ionicScrollDelegate.$getByHandle('scrollHandle' + slide).getScrollPosition().zoom;
      if (zoomFactor == $scope.zoomMin) {
        $ionicSlideBoxDelegate.enableSlide(true);
      } else {
        $ionicSlideBoxDelegate.enableSlide(false);
      }
    };
  })

  //.controller('PlaylistCtrl', function ($scope, $stateParams) {
  //  $scope.showImages = function (index) {
  //    $scope.activeSlide = index;
  //    $scope.showModal('templates/gallery-zoomview.html');
  //  };

  //  $scope.showModal = function (templateUrl) {
  //    $ionicModal.fromTemplateUrl(templateUrl, {
  //      scope: $scope
  //    }).then(function (modal) {
  //      $scope.modal = modal;
  //      $scope.modal.show();
  //    });
  //  }

  //  $scope.closeModal = function () {
  //    $scope.modal.hide();
  //    $scope.modal.remove()
  //  };

  //  $scope.updateSlideStatus = function (slide) {
  //    var zoomFactor = $ionicScrollDelegate.$getByHandle('scrollHandle' + slide).getScrollPosition().zoom;
  //    if (zoomFactor == $scope.zoomMin) {
  //      $ionicSlideBoxDelegate.enableSlide(true);
  //    } else {
  //      $ionicSlideBoxDelegate.enableSlide(false);
  //    }
  //  };
  //});
  .controller('SearchCtrl', function ($scope, $http, PaymentData, $state) {
    $scope.cards = [];
    $scope.FeeSelected = '';
    $scope.load = function () {
      $scope.cards.push({ DueDate: '15-11-2022', FeeName: 'Nov-2022', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-12-2022', FeeName: 'Term Fee I', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-12-2022', FeeName: 'Dec-2022', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-01-2023', FeeName: 'Jan-2023', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-02-2023', FeeName: 'Feb-2023', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-03-2023', FeeName: 'Mar-2023', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-03-2023', FeeName: 'Apr-2023', Amount: 1000 });
      $scope.cards.push({ DueDate: '15-03-2023', FeeName: 'May-2023', Amount: 1000 });
    };
    $scope.load();

    $scope.totalAmountSelected = function () {
      var totalAmount = 0;
      $scope.FeeSelected = '';
      for (var j = 0; j < $scope.cards.length; j++) {
        if ($scope.cards[j].Selected) {
          totalAmount += $scope.cards[j].Amount;
          $scope.FeeSelected += $scope.FeeSelected.length > 0 ? ', ' : '';
          $scope.FeeSelected += $scope.cards[j].FeeName;
        }
      }
      return totalAmount;
    };

    $scope.checkboxClick = function (c, $index) {
      var selected = c.Selected;
      for (var j = 0; j < $scope.cards.length; j++) {
        if (j <= $index && selected) {
          $scope.cards[j].Selected = selected;
        } else if (j > $index && !selected) {
          $scope.cards[j].Selected = selected;
        }
      }

    };

    $scope.payNow = function () {
      PaymentData.Amount = $scope.totalAmountSelected();
      PaymentData.Note = $scope.FeeSelected;
      $state.go("app.paymentStart")
    }

  })
  .controller('PaymentStartCtrl', function ($scope, $http, PaymentData, $interval, $state) {
    $scope.PaymentData = PaymentData;
    $scope.payData = [1];
    $scope.lastStatus = 'PAYMENT_SUCCESSFUL';
    $scope.payNow = function () {
      $http.post('http://localhost:51363/api/GeneratePaymentLink', JSON.stringify($scope.PaymentData)).then(function (response) {
        $scope.payData = [];
        if (response && response.data) {
          $scope.payData = response.data.split('`');
          console.log($scope.payData[0]);
          console.log($scope.payData[1]);
          $scope.checkStatus();
        }
      }, function (response) {
        $scope.payData = [];
        alert('Something went wrong!!');
        });

      
    }

    var stop;
    $scope.checkStatus = function () {
      if (angular.isDefined(stop)) return;
      $http.get('http://localhost:51363/api/GetToken').then(function (response) {
        if (response && response.data) {
          console.log(response.data);
        }
      }, function (response) {
        $scope.stopStatus();
      });
      stop = $interval(function () {
        $http.get('http://localhost:51363/api/CheckLinkStatus/' + $scope.payData[0]).then(function (response) {
          if (response && response.data) {
            $scope.lastStatus = response.data;
            if (response.data == 'PAYMENT_FAILED' || response.data == 'SETTLEMENT_SUCCESSFUL'
              || response.data == 'SETTLEMENT_FAILED' || response.data == 'BILL_EXPIRED') {
              $scope.stopStatus();
              if (response.data == 'SETTLEMENT_SUCCESSFUL' || response.data == 'PAYMENT_FAILED') {
                window.setTimeout(function () { $state.go("app.paymentStart"); }, 2000);
              }
            }
          }
        }, function (response) {
          $scope.stopStatus();
        });
      }, 10000);
    };
    $scope.stopStatus = function () {
      if (angular.isDefined(stop)) {
        $interval.cancel(stop);
        stop = undefined;
      }
    };
  });

