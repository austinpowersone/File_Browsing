angular.module('directory-info-app',[]).filter('urlencode',function()
{
	return function(input)
	{
		return window.encodeURIComponent(input);
	};
})
.controller('directory-info',
function($scope, $http)
{	
	var self = this;
	$scope.Directories = [];        	
	self.getDirectories = function(path)
	{				
		$scope.Directory = {
        	files: [],
        	path: null,
        	less: [],
        	more: [],
        	between: []
        };
        $("body").css("cursor", "wait");
		$http.get('/api/directoryinfo/' + path)
		.then(function successCallback(response)
		{								
			$scope.Directory.files = response.data.Files;
            $scope.Directory.path = response.data.Path;
            $scope.Directories = response.data.Directories;
            angular.forEach($scope.Directory.files, function(value, key)
            {            	
            	var value_mb = value.Size / 1024 / 1024;
            	if (value_mb <= 10)
            	{
            		$scope.Directory.less.push(value);
            	}
            	if ((value_mb > 10) && (value_mb <= 50))
            	{
            		$scope.Directory.between.push(value);
            	}
            	if (value_mb >= 100)
            	{
            		$scope.Directory.more.push(value);
            	}
            });
			$("body").css("cursor", "default");	
		}, function errorCallback(response)
		{
			console.log(response);
		});
	};

	$scope.folderClick = function (path)
	{
		self.getDirectories('?path=' + path);
	}

	self.getDirectories('');
});