<%@ Page Title="书籍上传" Language="C#" AutoEventWireup="true" CodeBehind="BookUpload.aspx.cs" Inherits="LibraryLink.Views.Admin.BookUpload" MasterPageFile="~/Views/Admin/AdminMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row mb-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        书籍上传
                    </div>
                    <div class="card-body">
   
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="txtBookName" class="form-label">书籍名称 <span id="BookNameTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtBookName" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtISBN" class="form-label">ISBN <span id="ISBNTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtPrice" class="form-label">价格 <span id="PriceTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-6">
                            <div class="col-md-6">
                                <label for="txtAuthor" class="form-label">作者名[国籍] <span id="AuthorTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control" placeholder="作者名[纯汉字国籍],多个作者之间以英文分号‘;’分隔"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtPublisher" class="form-label">出版社 <span id="PublisherTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtPublisher" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="calPublicationDate" class="form-label">出版日期 <span id="PublicationDateTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="calPublicationDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-3 mt-3">
                            <div class="col-md-6">
                                <label for="CoverImageUploader" class="form-label">封面图片 <span id="CoverImageTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:FileUpload ID="CoverImageUploader" runat="server" CssClass="form-control"></asp:FileUpload>
                            </div>
                            <div class="col-md-6">
                                <label for="BookFileUploader" class="form-label">书籍文件 (PDF) <span id="BookFileTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:FileUpload ID="BookFileUploader" runat="server" CssClass="form-control"></asp:FileUpload>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-12">
                                <label for="txtTags" class="form-label">标签 <span id="TagTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtTags" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="example : 标签A 标签B"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-12">
                                <label for="txtDescription" class="form-label">书籍描述 <span id="DescriptionTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="9"></asp:TextBox>
                            </div>
                        </div>

                        <asp:Button ID="btnSubmit" runat="server" Text="开始上传" CssClass="btn btn-primary btn-block" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        // 验证函数，匹配后端的验证规则
        function validInput() {
            let hasError = false;

            // 书籍名称验证
            const bookName = document.getElementById('<%= txtBookName.ClientID %>').value.trim();
            if (!/^[\sa-zA-Z0-9\u4e00-\u9fa5\(\)]+$/.test(bookName) || bookName === '') {
                document.getElementById('<%= BookNameTip.ClientID %>').innerText = "中英文开头，包含字母、数字、汉字、空格、括号";
                hasError = true;
            } else {
                document.getElementById('<%= BookNameTip.ClientID %>').innerText = "";
            }

            // ISBN验证
            const isbn = document.getElementById('<%= txtISBN.ClientID %>').value.trim();
            if (!/^\d{13}$/.test(isbn) || isbn === '') {
                document.getElementById('<%= ISBNTip.ClientID %>').innerText = "ISBN必须为13位纯数字";
                hasError = true;
            } else {
                document.getElementById('<%= ISBNTip.ClientID %>').innerText = "";
            }

            // 作者姓名验证
            const author = document.getElementById('<%= txtAuthor.ClientID %>').value.trim();
            if (!/^(?:[\u4e00-\u9fa5A-Za-z\s]+\[[\u4e00-\u9fa5]+\]\s*;\s*)+[\u4e00-\u9fa5A-Za-z\s]+\[[\u4e00-\u9fa5\s]+\]\s*$/.test(author) || author === '') {
                document.getElementById('<%= AuthorTip.ClientID %>').innerText = "人名含中英文字符和空格。国籍是方括号[]内的纯汉字，不含空格";
                hasError = true;
            } else {
                document.getElementById('<%= AuthorTip.ClientID %>').innerText = "";
            }

        

            // 出版社验证
            const publisher = document.getElementById('<%= txtPublisher.ClientID %>').value.trim();
            if (!/^[a-zA-Z\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5\s]+$/.test(publisher) || publisher === '') {
                document.getElementById('<%= PublisherTip.ClientID %>').innerText = "汉字、英文字母开头，空格分割单词"
                hasError = true;
            } else {
                document.getElementById('<%= PublisherTip.ClientID %>').innerText = "";
            }

            // 价格验证
            const price = document.getElementById('<%= txtPrice.ClientID %>').value.trim();
            if (price === '' || isNaN(price) || parseFloat(price) < 0 || parseFloat(price) > 99999999.99) {
                document.getElementById('<%= PriceTip.ClientID %>').innerText = "介于0~99999999.99间的阿拉伯数字";
                hasError = true;
            } else {
                document.getElementById('<%= PriceTip.ClientID %>').innerText = "";
            }

            // 标签验证
            const tags = document.getElementById('<%= txtTags.ClientID %>').value.trim().split(/\s+/);
            for (let tag of tags) {
                if (!/^((?:[a-zA-Z\u4e00-\u9fa5]+)(?:\s*))+$/.test(tag)) {
                    document.getElementById('<%= TagTip.ClientID %>').innerText = "标签只能包含中文、英文，以空格分割";
                    hasError = true;
                    break;
                } else {
                    document.getElementById('<%= TagTip.ClientID %>').innerText = "";
                }
            }

            // 书籍描述验证
            const description = document.getElementById('<%= txtDescription.ClientID %>').value.trim();
            if (description.length > 2000) {
                document.getElementById('<%= DescriptionTip.ClientID %>').innerText = "书籍描述不能超过2000字符";
                hasError = true;
            } else {
                document.getElementById('<%= DescriptionTip.ClientID %>').innerText = "";
            }

            return !hasError; // 返回false表示阻止提交
        }

        // 为所有输入框绑定oninput事件
        window.onload = function () {
            const inputs = document.querySelectorAll('input, textarea');
            inputs.forEach(input => {
                input.oninput = validInput;
            });
        };
    </script>



</asp:Content>

