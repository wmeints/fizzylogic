window.contentEditor = window.contentEditor || {};

(function(contentEditor) {
    /**
     * Uploads an image from the content editor.
     * @param {Blob} blob File to upload to the server.
     */
    contentEditor.uploadImage = function(blob) {
        var formData = new FormData();
        
        formData.append("file", blob);
        
        return fetch('/api/images', { method: 'POST', body: formData}).then((response) => {
            return response.json().then((responseData) => {
                return responseData.url;
            });
        });
    };

    /**
     * Reads the content from the content editor.
     */
    contentEditor.getContent = function () {
        return contentEditor.instance.getMarkdown();
    };

    /**
     * Sets the content for the content editor.
     * @param {string} value The markdown to set for the editor.
     */
    contentEditor.setContent = function (value) {
        contentEditor.instance.setMarkdown(value);
    };
    
    /**
     * Activates the content editor on the current page.
     * @param {string} content The markdown content to render.
     */
    contentEditor.activate = function(content) {
        contentEditor.instance = new toastui.Editor({
            el: document.querySelector('#editor'),
            previewStyle: 'vertical',
            height: '500px',
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
