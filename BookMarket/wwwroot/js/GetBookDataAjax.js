
function GetBookDataJson(IdBook, ChapterBook) {
    $.ajax({
                    type: "GET",
                    url: '/Books/GetDataBook',

                    data: {
                     page: ChapterBook, idBook: IdBook
                    },

                    success: function (data) {
                        
                        $("#content").html(data);
                    }
                });

}
