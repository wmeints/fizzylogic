window.contentEditor = window.contentEditor || {};

(function(contentEditor) {
    contentEditor.uploadImage = function(blob) {
        var formData = new FormData();
        
        formData.append("file", blob);
        
        return fetch('/api/images', { method: 'POST', body: formData}).then((response) => {
            return response.json().then((responseData) => {
                return responseData.url;
            });
        });
    };

    contentEditor.getContent = function () {
        return contentEditor.instance.getMarkdown();
    };

    contentEditor.setContent = function (value) {
        contentEditor.instance.setMarkdown(value);
    };
    
    contentEditor.activate = function(content) {
        contentEditor.instance = new toastui.Editor({
            el: document.querySelector('#editor'),
            previewStyle: 'vertical',
            height: 'auto',
            initialValue: content,
            previewStyle: 'tab',
            hooks: {
                addImageBlobHook: (blob, callback) => {
                    contentEditor.uploadImage(blob).then(url => {
                        callback(url, '');    
                    }, error => {
                        console.log("Image upload failed", error);
                    });
                }
            }
        });
    };
})(window.contentEditor);
